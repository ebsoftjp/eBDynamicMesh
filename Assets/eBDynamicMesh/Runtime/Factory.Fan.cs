using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public class FanData
        {
            public int radius = 90;
            public int maxY = 30;
            public float l1 = 1f;
            public Matrix4x4 m = Matrix4x4.identity;
        }

        public static Mesh CreateFan(string name, List<FanData> dataList)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();

            foreach (var data in dataList)
            {
                // circle
                data.radius = Mathf.Clamp(data.radius, 0, 360);
                var close = data.radius >= 360; // close over 360
                var maxY2 = data.maxY + (close ? 0 : 1);

                // center
                vertices.Add(data.m.MultiplyPoint3x4(Vector3.zero));

                // loop
                var vcount = vertices.Count;
                for (int y = 0; y < maxY2; y++)
                {
                    // center
                    var r1 = data.radius * y / data.maxY;
                    var m = data.m * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, r1), Vector3.one);
                    //var c = m.MultiplyPoint3x4(new Vector3(data.l1, 0, 0));
                    //var y1 = vcount + y * data.maxX;
                    //var y2 = vcount + ((y + 1) % maxY2) * data.maxX;
                    var isAddTriangles = y < data.maxY;
                    //var fy = (float)y / (float)(data.maxY - 1);

                    var v = m.MultiplyPoint3x4(new Vector3(data.l1, 0, 0));
                    vertices.Add(v);

                    if (isAddTriangles)
                    {
                        triangles.Add(0);
                        triangles.Add(vcount + y);
                        triangles.Add(vcount + y + 1);
                        triangles.Add(0);
                        triangles.Add(vcount + y + 1);
                        triangles.Add(vcount + y);
                    }
                }

                for (int i = 0; i < vertices.Count; i++)
                {
                    normals.Add(Vector3.forward);
                    uv.Add(vertices[i].normalized);
                    colors.Add(Color.white);
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
