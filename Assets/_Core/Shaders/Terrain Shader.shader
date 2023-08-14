Shader "Custom/Terrain Shader"
{
    Properties
    {
        testTexture("Texture", 2D) = "white"{}
        testScale("Scale", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float minHeight;
        float maxHeight;

        const static int maxLayerCount = 8;
        const static float epsilon = 1E-4;

        int layersCount;
        float3 baseColors[maxLayerCount];
        float baseStartHeights[maxLayerCount];
        float baseBlends[maxLayerCount];
        float baseColorStrenght[maxLayerCount];
        float baseTextureScales[maxLayerCount];

        sampler2D testTexture;
        float testScale;

        UNITY_DECLARE_TEX2DARRAY(baseTextures);
            
        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };

        float InverseLerp(float a, float b, float value)
        {
            return saturate((value - a) / (b - a));
        }

        float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex)
        {
            float3 scaledWorldPos = worldPos / scale;

            float3 xProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.y, scaledWorldPos.z, textureIndex)) * blendAxes.x;
            float3 yProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.z, textureIndex)) * blendAxes.y;
            float3 zProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, float3(scaledWorldPos.x, scaledWorldPos.y, textureIndex)) * blendAxes.z;

            return (xProjection + yProjection + zProjection);
        }


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 blendAxes = abs(IN.worldNormal);
            blendAxes /= blendAxes.x + blendAxes.y + blendAxes.z;

            float heightPercent = InverseLerp(minHeight, maxHeight, IN.worldPos.y);
            for (int i = 0; i < layersCount; i++)
            {
                float drawStr = InverseLerp(-baseBlends[i] / 2 - epsilon, baseBlends[i] / 2, heightPercent - baseStartHeights[i]);

                float3 baseColor = baseColors[i] * baseColorStrenght[i];
                float3 textureColor = triplanar(IN.worldPos, baseTextureScales[i], blendAxes, i) * (1 - baseColorStrenght[i]);

                o.Albedo = o.Albedo * (1 - drawStr) + (baseColor + textureColor) * drawStr;
            }

            //o.Albedo = xProjection + yProjection + zProjection;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
