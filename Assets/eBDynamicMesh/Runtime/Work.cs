using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace eBDynamicMesh
{
    public class Work
    {
        // data
        public int maxX = 10;
        public int maxY = 10;
        public float lenX = 0.5f;
        public float lenY = 1f;

        // vertex
        public List<Vector3> vertices = new();
        public List<Vector3> normals = new();
        public List<Vector2> uv = new();
        public List<Color> colors = new();
        public List<int> triangles = new();

        // skinned
        public List<Matrix4x4> bindposes = new();
        public List<BoneWeight> boneWeights = new();

        public Work AddBone(int n, float len)
        {
            // bindposes
            for (int i = 0; i < n; i++)
            {
                var t = (float)i / (n - 1) - 0.5f;
                var m = Matrix4x4.identity;
                m.SetTRS(
                    -new Vector3(0, t * len, 0),
                    Quaternion.identity,
                    Vector3.one);
                bindposes.Add(m);
            }

            return this;
        }

        public Work AddLine(float per1, float per2)
        {
            return Line.Add(this, maxX, maxY, lenX, lenX, lenY, per1, per2);
        }

        public Work AddCorn(float lenX1, float lenX2, float per1, float per2)
        {
            return Line.Add(this, maxX, maxY, lenX * lenX1, lenX * lenX2, lenY, per1, per2);
        }

        public Mesh ToMesh()
        {
            return new()
            {
                vertices = vertices.ToArray(),
                normals = normals.ToArray(),
                uv = uv.ToArray(),
                colors = colors.ToArray(),
                triangles = triangles.ToArray(),
                bindposes = bindposes.ToArray(),
                boneWeights = boneWeights.ToArray(),
            };
        }
    }
}
