using UnityEngine;

namespace DungeonMan.Terrain
{
    public class MapPreview : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public enum DrawMode { NoiseMap, MeshMap, FalloffMap };
        public DrawMode drawMode;

        public MeshObject meshObject;
        public HeightMapObject heightMapObject;
        public TextureObject textureObject;

        public Material terrainMaterial;

        [Range(0, MeshObject.numSupportedLODs - 1)]
        public int EditorPreviewLOD;

        [Space]
        public bool autoUpdate;

        public void DrawTexture(Texture2D texture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 6f;

            textureRender.gameObject.SetActive(true);
            meshFilter.gameObject.SetActive(false);
        }

        public void DrawMesh(MeshData meshData)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();

            textureRender.gameObject.SetActive(false);
            meshFilter.gameObject.SetActive(true);
        }


        void OnValuesUpdated()
        {
            if (!Application.isPlaying)
            {
                DrawMapinEditor();
            }
        }

        void OnTextureValuesUpdated()
        {
            textureObject.ApplyToMat(terrainMaterial);
        }

        public void DrawMapinEditor()
        {
            HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshObject.numVerticesPerLine, meshObject.numVerticesPerLine, heightMapObject, Vector2.zero);

            textureObject.ApplyToMat(terrainMaterial);
            textureObject.UpdateMeshHeights(terrainMaterial, heightMapObject.minHeight, heightMapObject.maxHeight);

            if (drawMode == DrawMode.NoiseMap)
            {
                //float[,] values = Noise.GenerateNoiseMap(meshObject.numVerticesPerLine, meshObject.numVerticesPerLine, heightMapObject.noiseSettings, Vector2.zero);
                DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
            }

            else if (drawMode == DrawMode.MeshMap)
            {
                DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshObject, EditorPreviewLOD));
            }

            else if (drawMode == DrawMode.FalloffMap)
            {
                DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffMapGenerator.GenerateFalloffMap(meshObject.numVerticesPerLine), 0, 1)));
            }
        }


        private void OnValidate()
        {
            if (meshObject != null)
            {
                meshObject.OnUpdate -= OnValuesUpdated;
                meshObject.OnUpdate += OnValuesUpdated;
            }

            if (heightMapObject != null)
            {
                heightMapObject.OnUpdate -= OnValuesUpdated;
                heightMapObject.OnUpdate += OnValuesUpdated;
            }

            if (textureObject != null)
            {
                textureObject.OnUpdate -= OnTextureValuesUpdated;
                textureObject.OnUpdate += OnTextureValuesUpdated;
            }
        }
    }
}