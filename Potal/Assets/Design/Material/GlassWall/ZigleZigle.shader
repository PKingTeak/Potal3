Shader "Custom/ZigleZigle"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (0.6, 0.8, 1, 0.3)
        _Distortion ("Distortion Strength", Range(0, 0.2)) = 0.005
        _Alpha ("Alpha", Range(0,1)) = 0.4
        _Speed ("Wave Speed", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent+10" "RenderType"="Transparent" }
        GrabPass { "_GrabTex" }

        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TintColor;
            float _Distortion;
            float _Alpha;
            float _Speed;
            sampler2D _GrabTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 grabUV : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabUV = ComputeGrabScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave = sin(i.uv.y * 40 + _Time.y * _Speed) * _Distortion;
                float2 uvDistort = i.grabUV.xy / i.grabUV.w + float2(0, wave * 0.5);

                fixed4 bg = tex2D(_GrabTex, uvDistort);
                fixed4 tint = _TintColor;

                // 배경 왜곡된 색과 tint 색을 섞기
                fixed4 finalColor = lerp(bg, tint, 0.5); // 0.5는 섞는 비율

                finalColor.a = _Alpha;
                return finalColor;
            }
            ENDCG
        }
    }
}
