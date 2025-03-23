Shader "MH_Custom/LitShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _BaseMap ("Base Texture", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        
        _SliceNormal("Slice Normal", Vector) = (0,0,0,0)
        _SliceCenter ("Slice Center", Vector) = (0,0,0,0)
        _SliceOffsetDst("Slice Offset", Float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Input properties
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
                float _Metallic;
                float _Smoothness;
            CBUFFER_END

            // World space normal of slice, anything along this direction from centre will be invisible
            float3 _SliceNormal;
            // World space centre of slice
            float3 _SliceCenter;
            // Increasing makes more of the mesh visible, decreasing makes less of the mesh visible
            float _SliceOffsetDst;

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            

            // Vertex input struct
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            // Fragment input struct
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float2 uv : TEXCOORD0;
            };

            // Vertex shader
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            // Fragment shader
            half4 frag(Varyings input) : SV_Target
            {
                float3 adjustedCentre = _SliceCenter + _SliceNormal * _SliceOffsetDst;
                float3 offsetTo_SliceCenter = adjustedCentre - input.positionWS;
                clip (dot(offsetTo_SliceCenter, _SliceNormal));
                
                // Sample the base texture
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // Get the main light
                Light mainLight = GetMainLight();
                float3 lightDir = mainLight.direction;
                half3 lightColor = mainLight.color;

                // Normalize normal
                float3 normalWS = normalize(input.normalWS);

                // Diffuse lighting (Lambertian)
                half NdotL = saturate(dot(normalWS, lightDir));
                half3 diffuse = albedo.rgb * lightColor * NdotL;

                // Specular (simple Blinn-Phong approximation)
                float3 viewDir = normalize(GetWorldSpaceViewDir(input.positionWS));
                float3 halfDir = normalize(lightDir + viewDir);
                half NdotH = saturate(dot(normalWS, halfDir));
                half specular = pow(NdotH, _Smoothness * 128.0) * _Metallic;
                half3 specularColor = specular * lightColor;

                // Ambient lighting (simple approximation)
                half3 ambient = albedo.rgb * 0.1;

                // Final color
                half3 finalColor = ambient + diffuse + specularColor;
                return half4(finalColor, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}