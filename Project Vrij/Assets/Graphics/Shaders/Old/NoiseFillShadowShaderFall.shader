Shader "Unlit/NoiseFillShadowShaderFall"
{
 Properties
 {
     _NoiseTex ("Noise Texture", 2D) = "white" {}
     _Color ("Base Color", Color) = (1,1,1,1)
     _NoiseColor ("Noise Color", Color) = (1,1,1,1)
     [MaterialToggle] _ScreenPosition ("Base Noise on Screen Position", Float) = 1
     _NoiseScalingObject ("Noise Scaling Object", Float) = 5
     _NoiseScalingScreen ("Noise Scaling Screen", Float) = .3
 }
 SubShader
 {
     Tags { 
     "RenderType"="Opaque" 
     "LightMode"="ForwardBase"
     }

     Pass
     {

         Cull Off
         Lighting On
         ZWrite On
 //        Blend SrcAlpha OneMinusSrcAlpha
         CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
//#pragma exclude_renderers d3d11 gles
         #pragma vertex vert
         #pragma fragment frag
         
         #include "UnityCG.cginc"
         #include "AutoLight.cginc"
         #pragma multi_compile_fwdbase

         struct appdata
         {
             float4 vertex : POSITION;
             float2 texcoord : TEXCOORD0;
             float3 normal : NORMAL;
         };

         struct v2f
         {
             float4 texcoord : TEXCOORD0;
             float4 pos : SV_POSITION;
             float intensity : TEXCOORD1;
             LIGHTING_COORDS(2,3)
         };

         float4 _NoiseColor;
         float4 _Color;
         
         v2f vert (appdata v)
         {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.texcoord.xy = v.texcoord;
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.intensity = nl;
            TRANSFER_VERTEX_TO_FRAGMENT(o);
            o.texcoord.zw = ComputeScreenPos(o.pos);
            return o;
         }


         sampler2D _NoiseTex;
         float _ScreenPosition;
         float _NoiseScalingObject;
         float _NoiseScalingScreen;
         
         fixed4 frag (v2f i) : SV_Target
         {
             fixed4 col = _Color;
            float attenuation = LIGHT_ATTENUATION(i);
            attenuation = min(attenuation, i.intensity);

            float overlayOpacity = tex2D(_NoiseTex, (i.texcoord.zw * (1/_NoiseScalingScreen)) %1024).r * _ScreenPosition;
            overlayOpacity += tex2D(_NoiseTex, (i.texcoord.xy / _NoiseScalingObject)).r * (1 - _ScreenPosition);

            overlayOpacity *= pow((1 - attenuation),2.2);
            col.rgb = (1 - overlayOpacity) * col.rgb + overlayOpacity * _NoiseColor.rgb;
            col.rgb *= col.a;
            return col;
         }
         ENDCG
     }
 }
     Fallback "VertexLit"
}
