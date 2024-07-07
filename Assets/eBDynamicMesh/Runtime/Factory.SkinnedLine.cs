using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public class SkinnedLineData
        {
            public int maxX = 8;
            public int maxY = 20;
            public float lenX = 0.1f;
            public float lenY = 1f;
            public int count = 6;
        }

        public static Mesh CreateSkinnedLine(string name, SkinnedLineData data)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();
            var bindposes = new List<Matrix4x4>();
            var boneWeights = new List<BoneWeight>();

            var indexes = new int[data.maxY + 1][];
            var color = Color.white;

            // bindposes
            var baseBone = bindposes.Count;
            for (int i = 0; i < data.count; i++)
            {
                var t = (float)i / (data.count - 1) - 0.5f;
                var m = Matrix4x4.identity;
                m.SetTRS(
                    -new Vector3(0, t * data.lenY, 0),
                    Quaternion.identity,
                    Vector3.one);
                bindposes.Add(m);
            }

            // loop
            for (int y = 0; y < data.maxY + 1; y++)
            {
                // center
                var fy = (float)y / data.maxY;
                var vy = (fy - 0.5f) * data.lenY;

                // bone
                var fb = fy * (data.count - 1);
                var nb = (int)fb;
                var boneWait = new BoneWeight
                {
                    boneIndex0 = baseBone + nb % data.count,
                    boneIndex1 = baseBone + (nb + 1) % data.count,
                    //boneIndex2 = 2,
                    //boneIndex3 = 3,
                    weight0 = 1f - (fb - nb),
                    weight1 = fb - nb,
                    //weight2 = (y % 4) == 2 ? 1 : 0,
                    //weight3 = (y % 4) == 3 ? 1 : 0,
                };

                indexes[y] = new int[data.maxX + 1];
                for (int x = 0; x < data.maxX + 1; x++)
                {
                    indexes[y][x] = vertices.Count;
                    var r2 = Mathf.Deg2Rad * x * 360 / data.maxX;
                    var v = new Vector3(Mathf.Cos(r2) * data.lenX, vy, Mathf.Sin(r2) * data.lenX);
                    vertices.Add(v);
                    normals.Add((v - Vector3.up * vy).normalized);
                    uv.Add(new(x / (float)data.maxX, fy));
                    colors.Add(color);
                    boneWeights.Add(boneWait);
                }
            }

            for (int y = 0; y < data.maxY; y++)
            {
                var y2 = y + 1;
                for (int x = 0; x < data.maxX; x++)
                {
                    var x2 = x + 1;
                    var n1 = indexes[y][x];
                    var n2 = indexes[y][x2];
                    var n3 = indexes[y2][x];
                    var n4 = indexes[y2][x2];
                    triangles.Add(n1);
                    triangles.Add(n3);
                    triangles.Add(n2);
                    triangles.Add(n2);
                    triangles.Add(n3);
                    triangles.Add(n4);
                }
            }

            return Add(name, new()
            {
                vertices = vertices.ToArray(),
                normals = normals.ToArray(),
                uv = uv.ToArray(),
                colors = colors.ToArray(),
                triangles = triangles.ToArray(),
                bindposes = bindposes.ToArray(),
                boneWeights = boneWeights.ToArray(),
            });
        }
    }
}
