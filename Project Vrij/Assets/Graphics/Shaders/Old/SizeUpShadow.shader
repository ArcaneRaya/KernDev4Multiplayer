Shader "Unlit/SizeUpShadow"
{
 Properties
 {
        // Shader properties
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
     _ShadowSize ("Shadow size", Float) = 2
     _ShadowFade ("Shadow Fade", Float) = .5
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

    Pass
    {
        Cull Off
        Lighting Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct vertexIN
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 texcoord: TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 texcoord: TEXCOORD0;
        };

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float _ShadowSize;
        float _ShadowFade;
        float4 _ShadowColor;

        float GetShadowMultiplier () 
        {
            return 1 + (_ShadowSize - 1) / 30;
        }

        v2f vert (vertexIN IN) 
        {
            v2f OUT;
            float3 worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
            float3 centerPos = mul(unity_ObjectToWorld, half4(0,0,0,1));
            OUT.vertex = UnityObjectToClipPos(IN.vertex * GetShadowMultiplier());
        //    OUT.vertex = UnityObjectToClipPos((worldPos - centerPos) * GetShadowMultiplier());
            OUT.color = IN.color;
            OUT.texcoord = IN.texcoord;
            return OUT;
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

        float GetAlphaAt (float2 position) 
        {
            float size = _ShadowSize;
            return GetNonAlphaRange(position, size);
        }

        float GetAlphaRange (float2 position, float dist) {
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

        float2 map (float2 position) {
            return position * GetShadowMultiplier() - (_ShadowSize -1) / 60;
        }

        float fract(float x) {
            return x - floor(x);
        }

        float rand(float2 co){
            return fract(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
        }

        fixed4 frag (v2f IN) : COLOR
        {
            float2 coord = map(IN.texcoord);
            float transparency = GetAlphaAt(coord);
            float randomAlpha = rand(coord);
            float surroundingAlpha = GetAlphaSet(coord);

            fixed4 color = _ShadowColor;
            color.a = min(min(transparency, randomAlpha), surroundingAlpha) * _ShadowColor.a;
            color.rgb *= color.a;

        //    if (color.a == 0) {
        //    color.a = 0.2;
         //   }
            return color;
        }


        ENDCG
    }

    Pass
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
         //   color.a = 0.5;

            if (_OutlineSize != 0){
                float isNotOutline = GetAlphaRange (IN.texcoord, _OutlineSize);
                color.rgb = isNotOutline * color.rgb + (1-isNotOutline) * _OutlineColor;
            }

            color.rgb *= color.a;
            return color;
        }

        ENDCG

    }
 }
}
