using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public static Mesh CreateCylinder(string name, int maxX, int maxY, float lenY, float lenX, bool topBottom = true)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();

            var indexes = new int[maxY + 1][];
            var color = Color.white;

            // loop
            for (int y = 0; y < maxY + 1; y++)
            {
                // center
                var fy = (float)y / maxY;
                var vy = (fy - 0.5f) * lenY;

                indexes[y] = new int[maxX + 1];
                for (int x = 0; x < maxX + 1; x++)
                {
                    indexes[y][x] = vertices.Count;
                    var r2 = Mathf.Deg2Rad * x * 360 / maxX;
                    var v = new Vector3(Mathf.Cos(r2) * lenX, vy, Mathf.Sin(r2) * lenX);
                    vertices.Add(v);
                    normals.Add((v - Vector3.up * vy).normalized);
                    uv.Add(new(x / (float)maxX, fy));
                    colors.Add(color);
                }
            }

            for (int y = 0; y < maxY; y++)
            {
                var y2 = y + 1;
                for (int x = 0; x < maxX; x++)
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

            void add(float vy, int offset, Vector3 normal, int n3, int n4)
            {
                var n1 = vertices.Count;
                vertices.Add(new(0, vy, 0));
                normals.Add(normal);
                uv.Add(new());
                colors.Add(color);
                for (int x = 0; x < maxX + 1; x++)
                {
                    var n2 = vertices.Count;
                    vertices.Add(vertices[x + offset]);
                    normals.Add(normal);
                    uv.Add(new());
                    colors.Add(color);
                    if (x > 0)
                    {
                        triangles.Add(n1);
                        triangles.Add(n2 + n3);
                        triangles.Add(n2 + n4);
                    }
                }
            }

            if (topBottom)
            {
                add((0 - 0.5f) * lenY, 0, Vector3.down, -1, 0);
                add((0 + 0.5f) * lenY, (maxX + 1) * maxY, Vector3.up, 0, -1);
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
