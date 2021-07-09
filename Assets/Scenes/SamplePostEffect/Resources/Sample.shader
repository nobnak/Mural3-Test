Shader "Unlit/Sample" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Throttle ("Throttle", Range(0, 1)) = 1
    }
    SubShader {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        ZTest Always ZWrite Off Cull Off

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D_X(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float _Throttle;
            CBUFFER_END

            v2f vert (appdata v) {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float4 cmain = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, i.uv);
                float lm = dot(float3(0.2, 0.7, 0.1), cmain.xyz);
                //float4 cout = float4(saturate(1 - cmain.xyz), cmain.w);
                float4 cout = float4(lm.xxx, cmain.w);

                cout = lerp(cmain, cout, _Throttle);
                return cout;
            }
            ENDHLSL
        }
    }
}
