Shader "Custom/PortalUnlitFX_NoEdgeFade_Red"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0,1)) = 1
        _DistortStrength ("Distortion", Range(0, 0.2)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Alpha;
            float _DistortStrength;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float2 dirToCenter = center - i.uv;

                float t = _Time.y * 2.0;
                float distort = sin(t + length(dirToCenter) * 20.0) * _DistortStrength;

                float2 distortedUV = i.uv + dirToCenter * distort;

                fixed4 col = tex2D(_MainTex, distortedUV);

                col.a *= _Alpha;

                return col;
            }
            ENDCG
        }
    }
}
