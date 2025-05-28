Shader "Custom/PortalCubeShader"
{
    Properties
    {
        _BumpMap ("Normal Map", 2D) = "bump" {} 
        _MainTex ("Base (RGB)", 2D) = "white" {}                         // 텍스처
        _EmissionMask ("Emission Mask", 2D) = "white" {}                 // 발광 영역 흑백 마스크
        _EmissionColor ("Glow Color", Color) = (0.1, 0.4, 1, 1)           // 발광 색상
        _EmissionStrength ("Glow Strength", Range(0,10)) = 2             // 발광 강도
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _EmissionMask;

        fixed4 _EmissionColor;
        float _EmissionStrength;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // 기본 텍스처
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            // 마스크 텍스처에서 발광 영역 정보 가져오기
            fixed4 mask = tex2D(_EmissionMask, IN.uv_MainTex);

            // 마스크 영역만 발광 + 색상 + 강도 반영
            o.Emission = _EmissionColor.rgb * _EmissionStrength * mask.r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
