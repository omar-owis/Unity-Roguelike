using UnityEngine;

namespace DungeonMan.Terrain
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, MeshObject meshSettings, int lof)
        {
            int meshIncrement = (lof == 0) ? 1 : 2 * lof;
            int borderedSize = heightMap.GetLength(0);
            int meshSize = borderedSize - 2 * meshIncrement;
            int meshSizeUnsimplified = borderedSize - 2;
            float topLeftX = (meshSizeUnsimplified - 1) / -2f;
            float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

            int verticesPerLine = (meshSize - 1) / meshIncrement + 1;

            MeshData meshData = new MeshData(borderedSize);

            int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
            int meshVertexIndex = 0;
            int borderVertexIndex = -1;

            for (int y = 0; y < borderedSize; y += meshIncrement)
            {
                for (int x = 0; x < borderedSize; x += meshIncrement)
                {
                    bool isBorderVertex = (y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1);

                    if (isBorderVertex)
                    {
                        vertexIndicesMap[x, y] = borderVertexIndex;
                        borderVertexIndex--;
                    }
                    else
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }
                }
            }

            for (int y = 0; y < borderedSize; y += meshIncrement)
            {
                for (int x = 0; x < borderedSize; x += meshIncrement)
                {
                    int vertexIndex = vertexIndicesMap[x, y];
                    Vector2 percent = new Vector2((x - meshIncrement) / (float)meshSize, (y - meshIncrement) / (float)meshSize);
                    float height = heightMap[x, y];
                    Vector3 vertexPositon = new Vector3((topLeftX + percent.x * meshSizeUnsimplified) * meshSettings.scale, height, (topLeftZ - percent.y * meshSizeUnsimplified) * meshSettings.scale);

                    meshData.AddVertex(vertexPositon, percent, vertexIndex);

                    if (x < borderedSize - 1 && y < borderedSize - 1)
                    {
                        int a = vertexIndicesMap[x, y];
                        int b = vertexIndicesMap[x + meshIncrement, y];
                        int c = vertexIndicesMap[x, y + meshIncrement];
                        int d = vertexIndicesMap[x + meshIncrement, y + meshIncrement];
                        meshData.AddTriangle(a, d, c);
                        meshData.AddTriangle(d, a, b);
                    }

                    vertexIndex++;
                }
            }

            meshData.BakeNormals();

            return meshData;

        }
    }
    public class MeshData
    {
        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;
        Vector3[] bakedNormals;

        Vector3[] borderVertices;
        int[] borderTriangles;

        int triangleIndex;
        int borderTriangleIndex;

        public MeshData(int VerticesPerLine)
        {
            vertices = new Vector3[VerticesPerLine * VerticesPerLine];
            uvs = new Vector2[VerticesPerLine * VerticesPerLine];
            triangles = new int[(VerticesPerLine - 1) * (VerticesPerLine - 1) * 6];

            borderVertices = new Vector3[4 * VerticesPerLine + 4];
            borderTriangles = new int[24 * VerticesPerLine];
        }

        public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
        {
            if (vertexIndex < 0)
            {
                borderVertices[-vertexIndex - 1] = vertexPosition;
            }
            else
            {
                vertices[vertexIndex] = vertexPosition;
                uvs[vertexIndex] = uv;
            }
        }

        public void AddTriangle(int a, int b, int c)
        {
            if (a < 0 || b < 0 || c < 0)
            {
                borderTriangles[borderTriangleIndex++] = a;
                borderTriangles[borderTriangleIndex++] = b;
                borderTriangles[borderTriangleIndex++] = c;
            }
            else
            {
                triangles[triangleIndex++] = a;
                triangles[triangleIndex++] = b;
                triangles[triangleIndex++] = c;
            }

        }

        Vector3[] CalculateNormals()
        {
            Vector3[] vertexNormals = new Vector3[vertices.Length];

            int triangleCount = triangles.Length / 3;

            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = triangles[normalTriangleIndex];
                int vertexIndexB = triangles[normalTriangleIndex + 1];
                int vertexIndexC = triangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;

            }


            int borderTriangleCount = borderTriangles.Length / 3;

            for (int i = 0; i < borderTriangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = borderTriangles[normalTriangleIndex];
                int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
                int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

                if (vertexIndexA >= 0) vertexNormals[vertexIndexA] += triangleNormal;
                if (vertexIndexB >= 0) vertexNormals[vertexIndexB] += triangleNormal;
                if (vertexIndexC >= 0) vertexNormals[vertexIndexC] += triangleNormal;
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            Vector3 pointA = (indexA < 0) ? borderVertices[-indexA - 1] : vertices[indexA];
            Vector3 pointB = (indexB < 0) ? borderVertices[-indexB - 1] : vertices[indexB];
            Vector3 pointC = (indexC < 0) ? borderVertices[-indexC - 1] : vertices[indexC];

            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;

            return Vector3.Cross(sideAB, sideAC).normalized;
        }

        public void BakeNormals()
        {
            bakedNormals = CalculateNormals();
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.normals = bakedNormals;
            return mesh;
        }
    }
}