Shader "Unlit/NoiseFillShader"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "white" {}
     _Color ("FadeColor", Color) = (1,1,1,1)
     _OutlineSize ("Outline Size", Float) = 2
     _OutlineColor ("Outline Color", Color) = (0,0,0,1)
     [MaterialToggle] _useGradient ("Use Gradient", Float) = 1
 }
 SubShader
 {
     Tags { "RenderType"="Transparent" }

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
         float _useGradient;
         float _OutlineSize;
         float4 _OutlineColor;
         
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
         
         fixed4 frag (v2f i) : SV_Target
         {
             fixed4 col = tex2D(_MainTex, i.uv);
             float overlayOpacity = rand(i.uv) * (0.8 - i.uv.y) * _useGradient;
             col.rgb = (1 - overlayOpacity) * col.rgb + overlayOpacity * _Color.rgb;
             float isNotOutline = GetAlphaRange(i.uv, _OutlineSize);
             col.rgb = isNotOutline * col.rgb + (1-isNotOutline) * _OutlineColor.rgb;

             col.rgb *= col.a;
             return col;
         }
         ENDCG
     }
 }
}
