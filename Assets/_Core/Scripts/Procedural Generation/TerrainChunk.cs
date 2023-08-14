using System;
using UnityEngine;

namespace DungeonMan.Terrain
{
    public class TerrainChunk
    {
        public event Action<TerrainChunk, bool> onVisiablityChanged;
        const float colliderGenerationDstThreshold = 5f;

        public Vector2 cord;

        GameObject meshObj;
        Vector2 sampleCenter;
        Bounds bounds;

        LODinfo[] detailLevels;
        LODMesh[] LODMeshes;
        int colliderLODIndex;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        HeightMap heightMap;
        bool mapDataReceived;
        int previousLODIndex = -1;
        bool hasSetCollider;
        float maxViewDst;

        HeightMapObject heightMapSettings;
        MeshObject meshSettings;

        Transform player;

        public TerrainChunk(Vector2 _cord, HeightMapObject heightMapSettings, MeshObject meshSettings, LODinfo[] detailLvls,
            int _colliderLODIndex, Transform parent, Material material, Transform player)
        {
            cord = _cord;
            sampleCenter = _cord * meshSettings.meshWorldSize / meshSettings.scale;
            detailLevels = detailLvls;
            colliderLODIndex = _colliderLODIndex;
            Vector2 pos = cord * meshSettings.meshWorldSize;
            bounds = new Bounds(pos, Vector2.one * meshSettings.meshWorldSize);

            this.heightMapSettings = heightMapSettings;
            this.meshSettings = meshSettings;
            this.player = player;

            meshObj = new GameObject("Chunk");
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshCollider = meshObj.AddComponent<MeshCollider>();
            int GroundLayer = LayerMask.NameToLayer("WhatIsGround");
            meshObj.layer = GroundLayer;

            meshObj.transform.position = new Vector3(pos.x, 0, pos.y);
            //meshObj.transform.localScale = Vector3.one * mapGenerator.meshObject.scale;
            meshRenderer.material = material;
            meshObj.transform.parent = parent;
            SetVisiable(false);

            LODMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < LODMeshes.Length; i++)
            {
                LODMeshes[i] = new LODMesh(detailLevels[i].lod);
                LODMeshes[i].updateCallback += UpdateTerrainChunk;
                if (i == colliderLODIndex) LODMeshes[i].updateCallback += UpdateCollisonMesh;
            }

            maxViewDst = detailLevels[detailLevels.Length - 1].dstThreshold;

        }

        public void Load()
        {
            ThreadedDataRequester.RequestData(OnHeightMapReceived,
                () => HeightMapGenerator.GenerateHeightMap(meshSettings.numVerticesPerLine, meshSettings.numVerticesPerLine, heightMapSettings, sampleCenter));
        }

        Vector2 PlayerPosition
        {
            get
            {
                return new Vector2(player.position.x, player.position.z);
            }
        }

        void OnHeightMapReceived(object _mapData)
        {
            heightMap = (HeightMap)_mapData;
            mapDataReceived = true;

            UpdateTerrainChunk();
        }
        Vector2 playerPosition
        {
            get
            {
                return new Vector2(player.position.x, player.position.z);
            }
        }

        public void UpdateTerrainChunk()
        {
            if (mapDataReceived)
            {
                bool wasVisiable = isVisiable();
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
                bool visiable = viewerDstFromNearestEdge <= maxViewDst;

                if (visiable)
                {
                    int LODIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].dstThreshold)
                        {
                            LODIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (LODIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = LODMeshes[LODIndex];
                        if (lodMesh.hasMesh)
                        {
                            previousLODIndex = LODIndex;
                            meshFilter.mesh = lodMesh.mesh;
                            //meshCollider.sharedMesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(heightMap, meshSettings);
                        }
                    }
                }

                if (wasVisiable != visiable)
                {
                    SetVisiable(visiable);
                    if (onVisiablityChanged != null) onVisiablityChanged(this, visiable);
                }

            }
        }

        public void UpdateCollisonMesh()
        {
            if (!hasSetCollider)
            {
                float sqrDstFromPlayerToEdge = bounds.SqrDistance(playerPosition);

                if (sqrDstFromPlayerToEdge < detailLevels[colliderLODIndex].sqrVisiableDstThreshold)
                {
                    if (!LODMeshes[colliderLODIndex].hasRequestedMesh)
                    {
                        LODMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
                    }
                }

                if (sqrDstFromPlayerToEdge < colliderGenerationDstThreshold * colliderGenerationDstThreshold)
                {
                    if (LODMeshes[colliderLODIndex].hasMesh)
                    {
                        meshCollider.sharedMesh = LODMeshes[colliderLODIndex].mesh;
                        hasSetCollider = true;
                    }

                }
            }
        }

        public void SetVisiable(bool _visiable)
        {
            meshObj.SetActive(_visiable);
        }

        public bool isVisiable()
        {
            return meshObj.activeSelf;
        }
    }
}