using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DungeonMan.Terrain
{
    [CreateAssetMenu(fileName = "New Texture Object", menuName = "Data/Procedural Terrain/Texture")]
    public class TextureObject : UpdateObject
    {
        const int textureSize = 512;
        const TextureFormat textureFormat = TextureFormat.RGB565;

        public Layer[] layers;

        float savedMinHeight;
        float savedMaxHeight;

        public void ApplyToMat(Material material)
        {
            material.SetInt("layersCount", layers.Length);
            material.SetColorArray("baseColors", layers.Select(x => x.tint).ToArray());
            material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
            material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrenght).ToArray());
            material.SetFloatArray("baseColorStrenght", layers.Select(x => x.tintStrenght).ToArray());
            material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());

            Texture2DArray texture2DArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());

            material.SetTexture("baseTextures", texture2DArray);

            UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
        }

        public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
        {
            savedMinHeight = minHeight;
            savedMaxHeight = maxHeight;

            material.SetFloat("minHeight", minHeight);
            material.SetFloat("maxHeight", maxHeight);
        }

        Texture2DArray GenerateTextureArray(Texture2D[] textures)
        {
            Texture2DArray texture2DArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);

            for (int i = 0; i < textures.Length; i++)
            {
                texture2DArray.SetPixels(textures[i].GetPixels(), i);
            }

            texture2DArray.Apply();

            return texture2DArray;
        }

        [Serializable]
        public class Layer
        {
            public Texture2D texture;
            public Color tint;

            [Range(0, 1)]
            public float tintStrenght;

            [Range(0, 1)]
            public float startHeight;

            [Range(0, 1)]
            public float blendStrenght;
            public float textureScale;
        }
    }
}