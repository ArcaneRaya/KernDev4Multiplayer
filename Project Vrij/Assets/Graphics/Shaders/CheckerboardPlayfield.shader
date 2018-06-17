Shader "Unlit/CheckerboardPlayfield"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "white" {}
        _ColorLight ("LightColor", Color) = (1,1,1,1)
        _ColorDark ("DarkColor", Color) = (1,1,1,1)
 }
 SubShader
 {
     Tags { "RenderType"="Opaque" "DisableBatching" = "True" }
     LOD 100

     Pass
     {
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         
         #include "UnityCG.cginc"

         struct appdata
         {
             float4 vertex : POSITION;
         };

         struct v2f
         {
             float4 vertex : SV_POSITION;
             float3 normal : NORMAL;
             float4 worldPos : TEXCOORD1;
         };

         sampler2D _MainTex;
         float4 _MainTex_ST;
         float4 _ColorLight;
         float4 _ColorDark;
         
         v2f vert (appdata v, float3 normal : NORMAL)
         {
             v2f o;
             o.vertex = UnityObjectToClipPos(v.vertex);
             o.normal = UnityObjectToWorldNormal(normal);
             o.worldPos = mul(unity_ObjectToWorld,float4(0,0,0,1));
             return o;
         }
         
         fixed4 frag (v2f i) : SV_Target
         {
             float dir = abs(round(i.normal.x));
             float darknessModifier = abs((round(i.worldPos.z) + round(i.worldPos.x))) % 2;

             fixed4 col = _ColorLight * dir + _ColorDark * abs(1 - dir);
             col *= (darknessModifier + 9) / 10;

             return col;
         }
         ENDCG
     }
 }
}
