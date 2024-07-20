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
        public Vector3 scale = Vector3.one;
        public Color color = Color.white;

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

        public Work SetLength(float lenX, float lenY)
        {
            this.lenX = lenX;
            this.lenY = lenY;
            return this;
        }

        public Work SetScale(float x, float y, float z) => SetScale(new(x, y, z));
        public Work SetScale(Vector3 scale)
        {
            this.scale = scale;
            return this;
        }

        public Work SetColor(Color color)
        {
            this.color = color;
            return this;
        }

        public Work AddLine(float per1, float per2)
        {
            return Line.Add(this, maxX, maxY, lenX / 2, lenX / 2, lenY, per1, per2);
        }

        public Work AddCorn(float per1, float per2, float lenX1, float lenX2)
        {
            return Line.Add(this, maxX, maxY, lenX * lenX1 / 2, lenX * lenX2 / 2, lenY, per1, per2);
        }

        public Work AddSphere(float per1, float per2, float sp1, float sp2)
        {
            return Sphere.Add(this, maxX, maxY, sp1, sp2, per1, per2);
        }

        public void AddWeight(float vy2, int maxX2)
        {
            if (bindposes.Count > 0)
            {
                var nb = 1;
                for (; nb < bindposes.Count - 1; nb++)
                {
                    if (vy2 < -bindposes[nb].GetPosition().y)
                    {
                        break;
                    }
                }
                var y0 = -bindposes[nb - 1].GetPosition().y;
                var y1 = -bindposes[nb].GetPosition().y;
                var fb = Mathf.Clamp01(Mathf.InverseLerp(y0, y1, vy2));
                var boneWait = new BoneWeight
                {
                    boneIndex0 = nb - 1,
                    boneIndex1 = nb,
                    //boneIndex2 = 2,
                    //boneIndex3 = 3,
                    weight0 = 1f - fb,
                    weight1 = fb,
                    //weight2 = (y % 4) == 2 ? 1 : 0,
                    //weight3 = (y % 4) == 3 ? 1 : 0,
                };
                for (int x = 0; x < maxX2; x++)
                {
                    boneWeights.Add(boneWait);
                }
            }
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
