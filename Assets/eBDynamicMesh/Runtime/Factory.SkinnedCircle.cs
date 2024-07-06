using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public class SkinnedCircleData
        {
            public int radius = 360;
            public int maxX = 28;
            public int maxY = 30;
            public float l1 = 0.5f;
            public float l2 = 0.25f;
            public Matrix4x4 m = Matrix4x4.identity;
            public int count = 6;
        }

        public static Mesh CreateSkinnedCircle(string name, List<SkinnedCircleData> dataList)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();
            var bindposes = new List<Matrix4x4>();
            var boneWeights = new List<BoneWeight>();

            foreach (var data in dataList)
            {
                // bindposes
                var baseBone = bindposes.Count;
                for (int i = 0; i < data.count; i++)
                {
                    var r = (i * 360 / data.count) * Mathf.Deg2Rad;
                    var m = Matrix4x4.identity;
                    m.SetTRS(
                        -new Vector3(Mathf.Cos(r) * data.l1, Mathf.Sin(r) * data.l1, 0),
                        Quaternion.identity,
                        Vector3.one);
                    bindposes.Add(m);
                }

                // pipe
                data.radius = Mathf.Clamp(data.radius, 0, 360);
                var maxX2 = data.maxX + 1;
                var maxY2 = data.maxY + 1;

                // loop
                var vcount = vertices.Count;
                for (int y = 0; y < maxY2; y++)
                {
                    // center
                    var r1 = data.radius * y / data.maxY;
                    var m = data.m * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, r1), Vector3.one);
                    var c = m.MultiplyPoint3x4(new Vector3(data.l1, 0, 0));
                    var y1 = vcount + y * maxX2;
                    var y2 = vcount + ((y + 1) % maxY2) * maxX2;
                    var isAddTriangles = y < data.maxY;
                    var fy = (float)y / (float)data.maxY;

                    // bone
                    var fb = fy * data.count;
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

                    for (int x = 0; x < maxX2; x++)
                    {
                        var r2 = (x * 360 / data.maxX) * Mathf.Deg2Rad;
                        var v = m.MultiplyPoint3x4(new Vector3(data.l1 + Mathf.Cos(r2) * data.l2, 0, Mathf.Sin(r2) * data.l2));
                        vertices.Add(v);
                        normals.Add((v - c).normalized);
                        uv.Add(new Vector2((float)x / (float)data.maxX, fy));
                        colors.Add(Color.white);
                        boneWeights.Add(boneWait);
                        if (isAddTriangles && x < data.maxX)
                        {
                            var x2 = x + 1;
                            triangles.Add(y1 + x);
                            triangles.Add(y2 + x);
                            triangles.Add(y1 + x2);
                            triangles.Add(y1 + x2);
                            triangles.Add(y2 + x);
                            triangles.Add(y2 + x2);
                        }
                    }
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
