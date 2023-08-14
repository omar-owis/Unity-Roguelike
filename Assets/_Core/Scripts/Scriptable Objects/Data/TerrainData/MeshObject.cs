using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMan.Terrain
{
    [CreateAssetMenu(fileName = "New Mesh Object", menuName = "Data/Procedural Terrain/Mesh")]
    public class MeshObject : UpdateObject
    {
        public const int numSupportedLODs = 5;
        public const int numOfSupportedChunkSizes = 9;
        public static readonly int[] supportedChunkSizes =
        {
        48, 72, 96, 120, 144, 168, 192, 216, 240
    };


        public float scale = 2.5f;

        [Range(0, numOfSupportedChunkSizes - 1)]
        public int chunkSizeIndex;

        public int numVerticesPerLine
        {
            get
            {
                return supportedChunkSizes[chunkSizeIndex] + 1;
            }
        }

        public float meshWorldSize
        {
            get
            {
                return (numVerticesPerLine - 3) * scale;
            }
        }
    }
}