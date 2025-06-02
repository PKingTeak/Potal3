
Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,1,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode"="Always" }

            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                float3 offset = norm * _OutlineWidth;
                float4 pos = v.vertex + float4(offset, 0);
                o.pos = UnityObjectToClipPos(pos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
