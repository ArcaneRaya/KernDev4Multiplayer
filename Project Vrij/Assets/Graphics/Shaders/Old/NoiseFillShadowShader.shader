Shader "Unlit/NoiseFillShadowShader"
{
 Properties
 {
     _Color ("Base Color", Color) = (1,1,1,1)
     _NoiseTex ("Noise", 2D) = "white" {}
     _NoiseColor ("Fade Color", Color) = (1,1,1,1)
     _Limit1 ("Limit 1", Float) = 0.3
     _Limit2 ("Limit 2", Float) = 0.7
     [MaterialToggle] _UseSteps ("Use Steps", Float) = 1
 }
 SubShader
 {
     Tags { 
     "RenderType"="Transparent" 
     "LightMode"="ForwardBase"
     }

     Pass
     {

         Cull Off
         Lighting On
         ZWrite On
         Blend SrcAlpha OneMinusSrcAlpha
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         
         #include "UnityCG.cginc"
         #include "UnityLightingCommon.cginc" // for _LightColor0

         struct appdata
         {
             float4 vertex : POSITION;
             float2 texcoord : TEXCOORD0;
             float2 uv : TEXCOORD1;
             float3 normal : NORMAL;
         };

         struct v2f
         {
             float2 texcoord : TEXCOORD0;
             float2 uv : TEXCOORD1;
             float4 vertex : SV_POSITION;
             float4 diff : COLOR0;
             float intensity : TEXCOORD2;
             float3 localPosition : TEXCOORD3;
         };

         float4 _NoiseColor;
         float4 _Color;
         float _Limit1;
         float _Limit2;
         float _UseSteps;
         
         v2f vert (appdata v)
         {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.texcoord = v.texcoord;
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float3 centerPos = mul(unity_ObjectToWorld, half4(0,0,0,1));
            o.localPosition = worldPos - centerPos;
            // get vertex normal in world space
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            // dot product between normal and light direction for
            // standard diffuse (Lambert) lighting
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.intensity = nl;
            o.diff = nl * _LightColor0;
        //    o.normal = v.normal;
             return o;
         }



         float fract(float x) {
            return x - floor(x);
         }

         float rand(float2 co){
                return fract(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }


         sampler2D _NoiseTex;
         
         fixed4 frag (v2f i) : SV_Target
         {
             fixed4 col = _Color;
             float overlayOpacity = rand(i.localPosition);

        //     float overlayOpacity = tex2D(_NoiseTex, i.texcoord).r;

             if (_UseSteps == 1) {
                if (i.intensity < _Limit2) {

                } else if (i.intensity < _Limit1) {
                    overlayOpacity *= 0.4f;
                } else {
                    overlayOpacity *= 0;
                }
                col.rgb = (1 - overlayOpacity) * col.rgb + overlayOpacity * _NoiseColor.rgb;
             } else {
                overlayOpacity *= (1 - i.intensity);
                col.rgb = (1 - overlayOpacity) * col.rgb + overlayOpacity * _NoiseColor.rgb;
             }


             /*  if (i.intensity < _Limit) {
                col.rgb = (1 - overlayOpacity) * col.rgb + overlayOpacity * _NoiseColor.rgb;
             }*/


             col.rgb *= col.a;
             return col;
         }
         ENDCG
     }
 }
}
