using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public static Mesh CreateCylinder(string name, int maxX, int maxY, float lenY, float lenX)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();

            var indexes = new int[maxY][];

            // loop
            for (int y = 0; y < maxY; y++)
            {
                // center
                var fy = (float)Mathf.Clamp(y - 1, 0, maxY - 3) / (maxY - 3);
                var vy = fy * lenY;
                var isEdge = y == 0 || y == maxY - 1;
                var maxX2 = isEdge ? 1 : maxX;
                //maxX2 = maxX;

                indexes[y] = new int[maxX2];
                for (int x = 0; x < maxX2; x++)
                {
                    indexes[y][x] = vertices.Count;
                    var r2 = x * 360 / maxX * Mathf.Deg2Rad;
                    var v = new Vector3(Mathf.Cos(r2) * lenX, vy, Mathf.Sin(r2) * lenX);
                    vertices.Add(v);
                    normals.Add((isEdge ? v : v - Vector3.up * vy).normalized);
                    uv.Add(new Vector2((float)x / (float)(maxX - 1), fy));
                    colors.Add(Color.white);
                }
            }

            for (int y = 0; y < indexes.Length - 1; y++)
            {
                var y2 = y + 1;
                var b1 = indexes[y].Length > 1;
                var b2 = indexes[y2].Length > 1;
                for (int x = 0; x < maxX; x++)
                {
                    var x2 = (x + 1) % maxX;
                    var n1 = indexes[y][b1 ? x : 0];
                    var n2 = indexes[y][b1 ? x2 : 0];
                    var n3 = indexes[y2][b2 ? x : 0];
                    var n4 = indexes[y2][b2 ? x2 : 0];
                    if (y > 0)
                    {
                        triangles.Add(n1);
                        triangles.Add(n3);
                        triangles.Add(n2);
                    }
                    if (y < maxY - 2)
                    {
                        triangles.Add(n2);
                        triangles.Add(n3);
                        triangles.Add(n4);
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
