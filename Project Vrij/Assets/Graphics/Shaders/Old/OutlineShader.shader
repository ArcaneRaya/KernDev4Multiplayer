// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlinedSprite"
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

        float Onified (float input) 
        {
            return input * (1 / abs(input));
        }

        float GetAlphaRange (float2 position, float dist) {
            float up = (position.y + _MainTex_TexelSize.y * dist) > 1 - _MainTex_TexelSize.y?
                0 : tex2D(_MainTex, position + fixed2(0, _MainTex_TexelSize.y * dist)).a;
            
            float down = (position.y - _MainTex_TexelSize.y * dist) < 0 + _MainTex_TexelSize.y?
                0 : tex2D(_MainTex, position - fixed2(0, _MainTex_TexelSize.y * dist)).a;

            float left = (position.x - _MainTex_TexelSize.x * dist) < 0 + _MainTex_TexelSize.x?
                0 : tex2D(_MainTex, position - fixed2(_MainTex_TexelSize.x * dist, 0)).a;

            float right = (position.x + _MainTex_TexelSize.x * dist) > 1 - _MainTex_TexelSize.x?
                0 :tex2D(_MainTex, position + fixed2(_MainTex_TexelSize.x * dist, 0)).a;
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