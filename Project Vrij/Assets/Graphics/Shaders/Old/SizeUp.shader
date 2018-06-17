// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SizeUp" 
{
 Properties
 {
        // Shader properties
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
     _ShadowSize ("Shadow size", Float) = 50
     _ShadowFade ("Shadow Fade", Float) = 1
     _OutlineSize ("Outline size", Float) = 2
     _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 }

 SubShader
 {
         Tags
         { 
             "Queue"="Transparent" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane"
             "CanUseSpriteAtlas"="True"
         }

        // Shader code
    Pass
        {
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct vertexIN 
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 texcoord : TEXCOORD0;
        };

        struct vertexToFragment
        {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 texcoord : TEXCOORD0;


            float3 worldPos : TEXCOORD1; // World position
            float3 centerPos : TEXCOORD2; // center position
            float sizeDiff : TEXCOORD3;
        };

        sampler2D _MainTex;
        float _TexWidth;
        float _TexHeight;
        float4 _ShadowColor;
        float4 _MainTex_TexelSize;
        float _ShadowSize;
        float _ShadowFade;
        float _UseShadow;

        vertexToFragment vert (vertexIN IN) 
        {
            vertexToFragment OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.color = IN.color;
            OUT.texcoord = IN.texcoord;

            OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
            OUT.centerPos = mul(unity_ObjectToWorld, half4(0,0,0,1));

            float3 enlargedPosition = (OUT.worldPos - OUT.centerPos) * _ShadowSize;
            OUT.sizeDiff = distance(OUT.vertex, UnityObjectToClipPos(enlargedPosition)) * _ShadowSize;

            OUT.vertex = UnityObjectToClipPos(enlargedPosition);

        //    OUT.vertex.x += (OUT.worldPos.x - OUT.centerPos.x) * _ShadowSize * (1 / _ScreenParams.x);
        //    OUT.vertex.y -= (OUT.worldPos.y - OUT.centerPos.y) * _ShadowSize * (1 / _ScreenParams.y);

            return OUT;
        }

        float fract(float x) {
            return x - floor(x);
        }

        float rand(float2 co){
            return fract(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
        }

        float GetNonAlphaRange (float2 position, float dist) {

            float up = tex2D(_MainTex, position + fixed2(0, _MainTex_TexelSize.y * dist)).a;;
            float down = tex2D(_MainTex, position - fixed2(0, _MainTex_TexelSize.y * dist)).a;
            float left = tex2D(_MainTex, position - fixed2(_MainTex_TexelSize.x * dist, 0)).a;
            float right = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, 0)).a;

            float botleft = tex2D(_MainTex, position + fixed2(-_MainTex_TexelSize.x * dist, _MainTex_TexelSize.y * dist)).a;
            float botright = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, _MainTex_TexelSize.y * dist)).a;
            float topleft = tex2D(_MainTex, position + fixed2(-_MainTex_TexelSize.x * dist, -_MainTex_TexelSize.y * dist)).a;
            float topright = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, -_MainTex_TexelSize.y * dist)).a;
            
            float sides = max(up, max(down, max(left, right)));
            float diagonals = max(topleft , max(topright, max(botleft, botright)));
            return max(sides,diagonals);
        }

        float GetAlphaAt (float2 position, float scale) {
            float textureAlpha = tex2D(_MainTex, position);
            float surroundingAlpha = GetNonAlphaRange (position, scale);
            return surroundingAlpha;
            return min(textureAlpha,surroundingAlpha);
        }

   /*     float GetAlphaRange (float2 position, float dist) {
            float up = GetAlphaAt(position + float2(0, _MainTex_TexelSize.y * dist));
            float down = GetAlphaAt(position + float2(0, _MainTex_TexelSize.y * dist));
            float left = GetAlphaAt(position + float2(_MainTex_TexelSize.x * dist, 0));
            float right = GetAlphaAt(position + float2(_MainTex_TexelSize.x * dist, 0));

            float botleft = GetAlphaAt(position + float2(-_MainTex_TexelSize.x * dist, _MainTex_TexelSize.y * dist));
            float botright = GetAlphaAt(position + float2(_MainTex_TexelSize.x * dist, _MainTex_TexelSize.y * dist));
            float topleft = GetAlphaAt(position + float2(-_MainTex_TexelSize.x * dist, -_MainTex_TexelSize.y * dist));
            float topright = GetAlphaAt(position + float2(_MainTex_TexelSize.x * dist, -_MainTex_TexelSize.y * dist));

            return up * down * left * right * botleft * botright * topleft * topright;
            return (up + down + left + right + botleft + botright + topright + topleft) / 8;
        }

         float GetAlphaSet (float2 position) {
            float multiplier = _ShadowSize / (_ShadowSize - (_ShadowSize * _ShadowFade));
             float dis1 = GetAlphaRange (position, 1 * multiplier);
             float dis2 = GetAlphaRange (position, 2 * multiplier);
             float dis3 = GetAlphaRange (position, 3 * multiplier);
             float dis4 = GetAlphaRange (position, 4 * multiplier);
             float dis5 = GetAlphaRange (position, 5 * multiplier);
             float dis6 = GetAlphaRange (position, 6 * multiplier);
             float dis7 = GetAlphaRange (position, 7 * multiplier);
             float dis8 = GetAlphaRange (position, 8 * multiplier);
             float resultAlpha = (dis1 + dis2 + dis3 + dis4 + dis5 + dis6 + dis7 + dis8) / 8;
             return resultAlpha;
         }
         
         */
        float2 map (float2 position) {
        /*
        float divider = 2;
            float slopeX = 1.0 * (1 + _MainTex_TexelSize.x * _ShadowSize / divider - -_MainTex_TexelSize.x * _ShadowSize / divider ) / (1 - 0);
            float newX = -_MainTex_TexelSize.x * _ShadowSize / divider  + slopeX * position.x;
            float slopeY = 1.0 * (1 + _MainTex_TexelSize.y * _ShadowSize / divider - -_MainTex_TexelSize.y * _ShadowSize / divider ) / (1 - 0);
            float newY = -_MainTex_TexelSize.y * _ShadowSize / divider  + slopeY * position.y;
            return float2(newX, newY);
            */

            return position * _ShadowSize - (_ShadowSize - 1) / 2;
        }

        half4 frag (vertexToFragment IN) : COLOR 
        {
            if (_ShadowSize == 0) {
                return fixed4(0,0,0,0);
            }

            float2 coord = IN.texcoord;
            coord = map(coord);

            fixed4 color = _ShadowColor;
            color.a = GetAlphaAt(coord, _ShadowSize * 25);
            float randomAlpha = rand(coord);
            float surroundingAlpha = 1; // GetAlphaSet(coord);
        //    color.a *= surroundingAlpha;
            color.a = min(min(color.a, randomAlpha), surroundingAlpha) * _ShadowColor.a;
            color.rgb *= color.a;


        //    fixed4 color = fixed4(1,1,1,1);
        //    color = tex2D(_MainTex, coord);
        //    color.rgb = 1;
            if (color.a == 0) {
                color.a = 0.1;
            }
            color.rgb *= color.a; 
            return color;
        }

        ENDCG
    }
 

/*   Pass
    {
        Cull Off
        Lighting Off
        ZWrite On
        Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct vertexIN 
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 texcoord : TEXCOORD0;
        };

        struct vertexToFragment
        {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 texcoord : TEXCOORD0;

            float3 centerPos : TEXCOORD1;
        };


        vertexToFragment vert (vertexIN IN) 
        {
            vertexToFragment OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.color = IN.color;
            OUT.texcoord = IN.texcoord;
            return OUT;
        }

        sampler2D _MainTex;
        float2 _MainTex_TexelSize;
        float _OutlineSize;
        float4 _OutlineColor;
        float4 _Color;

        float GetAlphaRange (float2 position, float dist) {
            float up = tex2D(_MainTex, position + fixed2(0, _MainTex_TexelSize.y * dist)).a;
            float down = tex2D(_MainTex, position - fixed2(0, _MainTex_TexelSize.y * dist)).a;
            float left = tex2D(_MainTex, position - fixed2(_MainTex_TexelSize.x * dist, 0)).a;
            float right = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, 0)).a;
            return up * down * left * right;
        }

        half4 frag (vertexToFragment IN) : COLOR 
        {
         //   float transparancy = tex2D(_MainTex, IN.texcoord).a;
            fixed4 color = tex2D(_MainTex, IN.texcoord) * _Color;
            color.a = 0.5;

            if (_OutlineSize != 0){
                float isNotOutline = GetAlphaRange (IN.texcoord, _OutlineSize);
                color.rgb = isNotOutline * color.rgb + (1-isNotOutline) * _OutlineColor;
            }

            color.rgb *= color.a;
            return color;
        }

        ENDCG

    }*/

 } 
}
