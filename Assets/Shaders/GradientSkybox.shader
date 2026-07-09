Shader "MechaBloom/GradientSkybox"
{
    Properties
    {
        _TopColor    ("Top Color", Color)    = (0.10, 0.16, 0.30, 1)
        _HorizonColor("Horizon Color", Color) = (0.30, 0.45, 0.55, 1)
        _BottomColor ("Bottom Color", Color)  = (0.06, 0.09, 0.14, 1)
        _HorizonSharp("Horizon Sharpness", Range(0.1, 8)) = 2.0
        _Exponent    ("Top Exponent", Range(0.2, 6)) = 1.4
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings   { float4 positionCS : SV_POSITION; float3 dir : TEXCOORD0; };

            float4 _TopColor;
            float4 _HorizonColor;
            float4 _BottomColor;
            float  _HorizonSharp;
            float  _Exponent;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.dir = IN.positionOS.xyz;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float h = normalize(IN.dir).y; // -1 bottom .. 1 top
                float up = pow(saturate(h), _Exponent);
                float down = pow(saturate(-h), _HorizonSharp);
                float3 col = lerp(_HorizonColor.rgb, _TopColor.rgb, up);
                col = lerp(col, _BottomColor.rgb, down);
                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}
