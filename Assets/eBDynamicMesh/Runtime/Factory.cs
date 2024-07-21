using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static Work CreateWork(string name = null)
        {
            var res = new Work();
            if (name != null) res.SetName(name);
            return res;
        }

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
            return GetWithGameObject(Get(name), material);
        }

        public static GameObject GetWithGameObject(Mesh mesh, Material material)
        {
            var obj = new GameObject(mesh.name);
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
                meshFilter.mesh = mesh;
            }

            return obj;
        }

        public static GameObject GetWithSkinnedObject(string name, Material material)
        {
            return GetWithSkinnedObject(Get(name), material);
        }

        public static GameObject GetWithSkinnedObject(Mesh mesh, Material material)
        {
            var obj = new GameObject(mesh.name);
            if (obj.GetComponent<SkinnedMeshRenderer>() == null)
            {
                // add mesh renderer
                var meshRenderer = obj.AddComponent<SkinnedMeshRenderer>();
                meshRenderer.sharedMesh = mesh;
                meshRenderer.rootBone = obj.transform;
                //meshRenderer.bones = new Transform[] { obj.transform };
                meshRenderer.bones = Enumerable
                    .Repeat(0, mesh.bindposeCount)
                    .Select((_, n) =>
                    {
                        var r = (n * 360 / mesh.bindposeCount) * Mathf.Deg2Rad;
                        var m = Matrix4x4.identity;
                        //var l2 = 0.5f;
                        //var pos = new Vector3(Mathf.Cos(r) * l2, Mathf.Sin(r) * l2, 0);
                        var pos = new Vector3(0, (float)n / (mesh.bindposeCount - 1) - 0.5f, 0);

                        var t = new GameObject($"{n}").transform;
                        t.SetParent(obj.transform);
                        t.SetLocalPositionAndRotation(pos, Quaternion.identity);
                        t.localScale = Vector3.one;
                        return t;
                    })
                    .ToArray();
                meshRenderer.material = material;
            }

            return obj;
        }

        public static Mesh Get(string name) => Instance.dic[name];
    }
}
