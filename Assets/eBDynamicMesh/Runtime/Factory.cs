using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        private static Factory instance;
        private static bool isQuit;

        public static Factory Instance
        {
            get
            {
                if (instance == null && !isQuit)
                {
                    instance = new();
                    Application.quitting += () =>
                    {
                        instance = null;
                        isQuit = true;
                    };
                }
                return instance;
            }
        }

        private readonly Dictionary<string, Mesh> dic = new();

        public static Mesh Add(string name, Mesh mesh)
        {
            Instance.dic[name] = mesh;
            return Instance.dic[name];
        }

        public static void AddBackTriangles(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uv, List<Color> colors, List<int> triangles)
        {
            var n1 = vertices.Count;
            Debug.Assert(n1 == normals.Count, $"normals is different: {n1} != {normals.Count}");
            Debug.Assert(n1 == uv.Count, $"uv is different: {n1} != {uv.Count}");
            Debug.Assert(n1 == colors.Count, $"colors is different: {n1} != {colors.Count}");
            for (int i = 0; i < n1; i++)
            {
                vertices.Add(vertices[i]);
                normals.Add(normals[i] * -1);
                uv.Add(uv[i]);
                colors.Add(colors[i]);
            }

            var n2 = triangles.Count;
            var triangles2 = new int[n2];
            for (int i = 0; i < n2; i += 3)
            {
                triangles2[i] = triangles[i] + n1;
                triangles2[i + 1] = triangles[i + 2] + n1;
                triangles2[i + 2] = triangles[i + 1] + n1;
            }
            triangles.InsertRange(0, triangles2);
        }

        public static GameObject GetWithGameObject(string name, Material material)
        {
            var obj = new GameObject(name);
            if (obj.GetComponent<MeshRenderer>() == null)
            {
                // add mesh renderer
                var meshRenderer = obj.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
            }

            if (obj.GetComponent<MeshFilter>() == null)
            {
                // add mesh filter
                var meshFilter = obj.AddComponent<MeshFilter>();
                meshFilter.mesh = Get(name);
            }

            return obj;
        }

        public static Mesh Get(string name) => Instance.dic[name];
    }
}
