Shader "Unlit/NoiseShadowShader"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "white" {}
     _Color ("Tint", Color) = (0,0,0,0)
     _Size ("Size", Float) = 2
 }
 SubShader
 {
     Tags { "RenderType"="Transparent" }

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

         struct appdata
         {
             float4 vertex : POSITION;
             float2 uv : TEXCOORD0;
         };

         struct v2f
         {
             float2 uv : TEXCOORD0;
             float4 vertex : SV_POSITION;
         };

         sampler2D _MainTex;
             float4 _MainTex_TexelSize;
         float4 _MainTex_ST;
         float4 _Color;
         float _Size;
         
         v2f vert (appdata v)
         {
             v2f o;
             o.vertex = UnityObjectToClipPos(v.vertex);
             o.uv = v.uv;
             return o;
         }

         float fract(float x) {
            return x - floor(x);
         }

         float rand(float2 co){
                return fract(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

         float GetAlphaRange (float2 position, float dist) {
            fixed4 up = fixed4(0, 0, 0, 0);
                 fixed4 down = fixed4(0, 0, 0, 0);
                 fixed4 left = fixed4(0, 0, 0, 0);
                 fixed4 right = fixed4(0, 0, 0, 0);
            fixed4 topleft = fixed4(0, 0, 0, 0);
                 fixed4 topright = fixed4(0, 0, 0, 0);
                 fixed4 botleft = fixed4(0, 0, 0, 0);
                 fixed4 botright = fixed4(0, 0, 0, 0);

                 if (position.y > _MainTex_TexelSize.y * (dist)) {
                    up = tex2D(_MainTex, position + fixed2(0, _MainTex_TexelSize.y * dist));;
                 }
                 if (position.y < 1- _MainTex_TexelSize.y * (dist)) {
                    down = tex2D(_MainTex, position - fixed2(0, _MainTex_TexelSize.y * dist));
                 }
                 if (position.x > _MainTex_TexelSize.x * (dist)) {
                    left = tex2D(_MainTex, position - fixed2(_MainTex_TexelSize.x * dist, 0));
                 }
                 if (position.x < 1 - _MainTex_TexelSize.x * dist) {
                    right = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, 0));
                 }

      /*           float cirkeloffset = 2;
                 if (position.y > _MainTex_TexelSize.y * cirkeloffset * (dist) && position.x > _MainTex_TexelSize.x * cirkeloffset * (dist)) {
                    topleft = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * cirkeloffset * dist,_MainTex_TexelSize.y * cirkeloffset * dist));
                 }
                 if (position.y > _MainTex_TexelSize.y * cirkeloffset * (dist) && position.x < 1 - _MainTex_TexelSize.x * cirkeloffset * (dist)) {
                    topright = tex2D(_MainTex, position + fixed2(-_MainTex_TexelSize.x * cirkeloffset * dist,_MainTex_TexelSize.y * cirkeloffset * dist));
                 }

                 if (position.y < 1- _MainTex_TexelSize.y * cirkeloffset * (dist) && position.x > _MainTex_TexelSize.x * cirkeloffset * (dist)) {
                    botleft = tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * cirkeloffset * dist,-_MainTex_TexelSize.y * cirkeloffset * dist));
                 }
                 if (position.y < 1- _MainTex_TexelSize.y * cirkeloffset * (dist) && position.x < 1 - _MainTex_TexelSize.x * cirkeloffset * (dist)) {
                    botright = tex2D(_MainTex, position + fixed2(-_MainTex_TexelSize.x * cirkeloffset * dist,-_MainTex_TexelSize.y * cirkeloffset * dist));
                 }*/
        //    return up.a * down.a * left.a * right.a * topleft * topright * botleft * botright;
            return up.a * down.a * left.a * right.a;
         }

         float GetAlphaSet (float2 position) {
             float dis1 = GetAlphaRange (position, 1 * _Size);
             float dis2 = GetAlphaRange (position, 2 * _Size);
             float dis3 = GetAlphaRange (position, 3 * _Size);
             float dis4 = GetAlphaRange (position, 4 * _Size);
             float dis5 = GetAlphaRange (position, 5 * _Size);
             float dis6 = GetAlphaRange (position, 6 * _Size);
             float dis7 = GetAlphaRange (position, 7 * _Size);
             float dis8 = GetAlphaRange (position, 8 * _Size);
             float resultAlpha = (dis1 + dis2 + dis3 + dis4 + dis5 + dis6 + dis7 + dis8) / 8;
             return resultAlpha;
         }
         
         fixed4 frag (v2f i) : SV_Target
         {
             fixed4 col = tex2D(_MainTex, i.uv);
             col.rgb = _Color.rgb;
           //  col.a = min(col.a, _Color.a);
             float resultAlpha = GetAlphaSet (i.uv);
             col.a = min(col.a, resultAlpha);
             float randomAlpha = rand(i.uv);
          //   float randomAlpha2 = rand(i.uv + float2(0, _MainTex_TexelSize.y));
           //  float randomAlpha3 = rand(i.uv + float2(_MainTex_TexelSize.x, 0));
            // float randomAlpha4 = rand(i.uv + _MainTex_TexelSize);

           //  randomAlpha = min(randomAlpha, min(randomAlpha2, min(randomAlpha3, randomAlpha4)));

             col.a = min(col.a, rand(i.uv));
             col.rgb *= col.a;
             col.a *= _Color.a;
             return col;
         }
         ENDCG
     }
 }
}
