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
