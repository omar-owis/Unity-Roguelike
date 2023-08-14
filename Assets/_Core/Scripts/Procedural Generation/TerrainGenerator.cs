using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMan.Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        const float chunkThresholdUpdate = 25f;
        const float sqrChunkThresholdUpdate = chunkThresholdUpdate * chunkThresholdUpdate;
        public int colliderLODIndex;

        public MeshObject meshSettings;
        public HeightMapObject heightMapObject;
        public TextureObject textureObject;

        public LODinfo[] detialLevels;

        public Transform player;
        public Material mapMaterial;

        Vector2 playerPos;
        Vector2 playerPosOld;

        float meshWorldSize;
        int visiableChunks;

        Dictionary<Vector2, TerrainChunk> terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();
        List<TerrainChunk> visiableTerrainChunks = new List<TerrainChunk>();

        private void Start()
        {
            float maxViewDst = detialLevels[detialLevels.Length - 1].dstThreshold;
            meshWorldSize = meshSettings.meshWorldSize;
            visiableChunks = Mathf.RoundToInt(maxViewDst / meshWorldSize);
            UpdateVisiableChunks();

            textureObject.ApplyToMat(mapMaterial);
            textureObject.UpdateMeshHeights(mapMaterial, heightMapObject.minHeight, heightMapObject.maxHeight);
        }

        private void Update()
        {
            playerPos = new Vector2(player.position.x, player.position.z);

            if (playerPos != playerPosOld)
            {
                foreach (TerrainChunk chunk in visiableTerrainChunks)
                {
                    chunk.UpdateCollisonMesh();
                }
            }

            if ((playerPosOld - playerPos).sqrMagnitude > sqrChunkThresholdUpdate)
            {
                playerPosOld = playerPos;
                UpdateVisiableChunks();
            }
        }

        void UpdateVisiableChunks()
        {
            HashSet<Vector2> alreadUpdatedChunkCords = new HashSet<Vector2>();
            for (int i = visiableTerrainChunks.Count - 1; i >= 0; i--)
            {
                alreadUpdatedChunkCords.Add(visiableTerrainChunks[i].cord);
                visiableTerrainChunks[i].UpdateTerrainChunk();
            }

            int currentChunkX = Mathf.RoundToInt(playerPos.x / meshWorldSize);
            int currentChunkY = Mathf.RoundToInt(playerPos.y / meshWorldSize);

            for (int yOffset = -visiableChunks; yOffset <= visiableChunks; yOffset++)
            {
                for (int xOffset = -visiableChunks; xOffset <= visiableChunks; xOffset++)
                {
                    Vector2 viewedChunkCord = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                    if (!alreadUpdatedChunkCords.Contains(viewedChunkCord))
                    {
                        if (terrainChunkDict.ContainsKey(viewedChunkCord))
                        {
                            terrainChunkDict[viewedChunkCord].UpdateTerrainChunk();
                        }
                        else
                        {
                            TerrainChunk newChunk = new TerrainChunk(viewedChunkCord, heightMapObject, meshSettings, detialLevels, colliderLODIndex, transform, mapMaterial, player);
                            terrainChunkDict.Add(viewedChunkCord, newChunk);
                            newChunk.onVisiablityChanged += OnChunkVisibilityChanged;
                            newChunk.Load();
                        }
                    }
                }
            }

            void OnChunkVisibilityChanged(TerrainChunk chunk, bool isVisiable)
            {
                if (isVisiable)
                {
                    visiableTerrainChunks.Add(chunk);
                }
                else
                {
                    visiableTerrainChunks.Remove(chunk);
                }
            }
        }
    }

    public class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        public event Action updateCallback;

        public LODMesh(int _lod)
        {
            lod = _lod;
        }

        void OnMeshDataReceived(object meshData)
        {
            mesh = ((MeshData)meshData).CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(HeightMap mapData, MeshObject meshSetting)
        {
            hasRequestedMesh = true;
            ThreadedDataRequester.RequestData(OnMeshDataReceived, () => MeshGenerator.GenerateTerrainMesh(mapData.values, meshSetting, lod));
        }
    }


    [Serializable]
    public struct LODinfo
    {
        [Range(0, MeshObject.numSupportedLODs - 1)]
        public int lod;
        public float dstThreshold;
        public float sqrVisiableDstThreshold
        {
            get
            {
                return dstThreshold * dstThreshold;
            }
        }
    }
}