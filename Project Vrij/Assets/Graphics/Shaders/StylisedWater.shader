Shader "Unlit/StylisedWater"
{
 Properties
 {
     _Color("Main Color", Color) = (1, 1, 1, 1)
     _WaveTexture ("Wave Texture", 2D) = "white" {}
     _WaveSpeed ("Wave Speed", Float) = 0.1
 }
 SubShader
 {
        Tags {"RenderType"="Opaque"  }
     LOD 100

     Pass
     {
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
             float2 realuv : TEXCOORD0;
             float2 localuv : TEXCOORD1;
             float2 uv : TEXCOORD2;
             float2 uv2 : TEXCOORD3;
             float4 vertex : SV_POSITION;
         };

         fixed4 _Color;
         sampler2D _CameraDepthNormalsTexture;
         float _IntersectionSize;
         float _WaveSpeed;
         sampler2D _WaveTexture;
         
         v2f vert (appdata v)
         {
             v2f o;
             o.vertex = UnityObjectToClipPos(v.vertex);

            // o.localuv = v.uv;
             o.localuv = v.uv;
             o.realuv = v.uv;

             // quarters
             o.localuv *= 2;
             o.localuv.xy -= 1;
             o.localuv.x = abs(o.localuv.x);
             o.localuv.y = abs(o.localuv.y);

             o.uv = o.localuv;
             o.uv2 = o.uv;

             // scrolling
             o.uv.xy = o.uv.xy - frac(_Time.y * _WaveSpeed);
             o.uv2.xy = o.uv2.xy - frac(_Time.y * _WaveSpeed * 1.3f) - float2(0.1,0.06);

             return o;
         }
         
         fixed4 frag (v2f i) : SV_Target
         {

            fixed4 col = _Color;
            col.rgb -= fixed3(0.4,0.4,0.4) * max(0, (.3 - distance(float2(0.5,0.5),i.realuv.xy)));

            float waveAlpha1 = tex2D(_WaveTexture, i.uv).a;
            float waveAlpha2 = tex2D(_WaveTexture, i.uv2).a;
            float totalAlpha = waveAlpha1 + waveAlpha2;
            totalAlpha /= 2;
            totalAlpha *= max(i.localuv.x, i.localuv.y);
            totalAlpha = lerp(0,1,pow(totalAlpha,4));

            return col * (1-totalAlpha) + fixed4(1,1,1,1) * totalAlpha;
         }
         ENDCG
     }
 }
}
