using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public class LineData
        {
            public int radius = 180;
            public int maxX = 28;
            public int maxY = 30;
            public float l1 = 0.5f;
            public float l2 = 0.5f;
            public Matrix4x4 m = Matrix4x4.identity;
        }

        public static Mesh CreateLine(string name, List<LineData> dataList)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();

            foreach (var data in dataList)
            {
                // pipe
                data.radius = Mathf.Clamp(data.radius, 0, 360);
                var close = data.radius >= 360; // close over 360
                var maxY2 = data.maxY + (close ? 0 : 1);

                // loop
                var vcount = vertices.Count;
                for (int y = 0; y < maxY2; y++)
                {
                    // center
                    var r1 = data.radius * y / data.maxY;
                    var m = data.m * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, r1), Vector3.one);
                    var c = m.MultiplyPoint3x4(new Vector3(data.l1, 0, 0));
                    var y1 = vcount + y * data.maxX;
                    var y2 = vcount + ((y + 1) % maxY2) * data.maxX;
                    var isAddTriangles = y < data.maxY;
                    var fy = (float)y / (float)(data.maxY - 1);

                    for (int x = 0; x < data.maxX; x++)
                    {
                        var r2 = (x * 360 / data.maxX) * Mathf.Deg2Rad;
                        var v = m.MultiplyPoint3x4(new Vector3(data.l1 + Mathf.Cos(r2) * data.l2, 0, Mathf.Sin(r2) * data.l2));
                        vertices.Add(v);
                        normals.Add((v - c).normalized);
                        uv.Add(new Vector2((float)x / (float)(data.maxX - 1), fy));
                        colors.Add(Color.white);

                        if (isAddTriangles)
                        {
                            var x2 = (x + 1) % data.maxX;
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
            });
        }
    }
}
