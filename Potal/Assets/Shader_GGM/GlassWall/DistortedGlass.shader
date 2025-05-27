Shader "Custom/GlassLitTint"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (0.6, 0.8, 1, 1)
        _Alpha ("Alpha", Range(0,1)) = 0.3
        _Glossiness ("Smoothness", Range(0,1)) = 1
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting On

        CGPROGRAM
        #pragma surface surf BlinnPhong alpha

        fixed4 _TintColor;
        fixed4 _MySpecColor;   // ← 여기 이름 정확히!
        float _Alpha;
        float _Glossiness;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _TintColor.rgb;
            o.Alpha = _Alpha;
            o.Specular = _SpecColor.r; // 일반적으로 RGB 중 R만 사용 (또는 평균)
            o.Gloss = _Glossiness;
            o.Emission = _TintColor.rgb * 0.1;
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
}
