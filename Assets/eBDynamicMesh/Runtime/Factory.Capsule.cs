using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public static Mesh CreateCapsule(string name, int maxX, int maxY, int maxY2, float radius, float len)
        {
            return CreateCapsule(name, maxX, maxY, maxY2, radius, len, Vector3.one);
        }

        public static Mesh CreateCapsule(string name, int maxX, int maxY, int maxY2, float radius, float len, Vector3 sc)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uv = new List<Vector2>();
            var colors = new List<Color>();
            var triangles = new List<int>();
            var color = Color.white;
            var cylLen = len - radius * 2;

            var indexes2 = new int[][]
            {
                new int[maxX + 1],
                new int[maxX + 1],
            };

            // loop
            for (int d = 0; d < 2; d++)
            {
                var indexes = new int[maxY][];
                var addY = new Vector3(0, d == 0 ? cylLen / 2 : -cylLen / 2, 0);

                for (int y = 0; y < maxY; y++)
                {
                    // center
                    var r01 = y * 90 / (maxY - 1);
                    if (d == 1) r01 += 90;
                    var r1 = r01 * Mathf.Deg2Rad;
                    //var r1 = (y + 1) * 180 / (maxY + 2 - 1) * Mathf.Deg2Rad;
                    //var fy = (float)y / (float)(maxY - 1);
                    var vy = Mathf.Cos(r1) * radius;
                    var l1 = Mathf.Sin(r1) * radius;
                    var maxX2 = (y == 0 && d == 0 || y == maxY - 1 && d == 1) ? 1 : maxX + 1;
                    //maxX2 = maxX;

                    indexes[y] = new int[maxX2];
                    for (int x = 0; x < maxX2; x++)
                    {
                        indexes[y][x] = vertices.Count;
                        if (y == 0 && d == 1 || y == maxY - 1 && d == 0) indexes2[d][x] = vertices.Count;
                        var r2 = x * 360 / maxX * Mathf.Deg2Rad;
                        var v = new Vector3(Mathf.Cos(r2) * l1, vy, Mathf.Sin(r2) * l1);
                        var v2 = v + addY;
                        vertices.Add(Vector3.Scale(v2, sc));
                        //normals.Add(v.normalized);
                        //normals.Add(Vector3.Scale(v, sc).normalized);
                        normals.Add(Vector3.Scale(v, new(sc.y * sc.z, sc.z * sc.x, sc.x * sc.y)).normalized);
                        //uv.Add(new Vector2((float)x / (float)maxX, 1 - fy));
                        uv.Add(new Vector2((float)x / (float)maxX, v2.y / len + 0.5f));
                        colors.Add(color);
                    }
                }

                for (int y = 0; y < indexes.Length - 1; y++)
                {
                    var y2 = y + 1;
                    var b1 = indexes[y].Length > 1;
                    var b2 = indexes[y2].Length > 1;
                    for (int x = 0; x < maxX; x++)
                    {
                        var x2 = x + 1;
                        var n1 = indexes[y][b1 ? x : 0];
                        var n2 = indexes[y][b1 ? x2 : 0];
                        var n3 = indexes[y2][b2 ? x : 0];
                        var n4 = indexes[y2][b2 ? x2 : 0];
                        if (!(y == 0 && d == 0))
                        {
                            triangles.Add(n1);
                            triangles.Add(n2);
                            triangles.Add(n3);
                        }
                        if (!(y == maxY - 1 && d == 1))
                        {
                            triangles.Add(n2);
                            triangles.Add(n4);
                            triangles.Add(n3);
                        }
                    }
                }
            }

            {
                maxY = maxY2;
                var lenX = radius;
                var lenY = cylLen;
                var indexes = new int[maxY + 1][];

                // loop
                for (int y = 0; y < maxY + 1; y++)
                {
                    indexes[y] = new int[maxX + 1];

                    var d = y == 0 ? 1 : (y == maxY ? 0 : -1);
                    if (d != -1)
                    {
                        for (int x = 0; x < maxX + 1; x++)
                        {
                            indexes[y][x] = indexes2[d][x];
                        }
                        continue;
                    }

                    // center
                    var fy = (float)y / maxY;
                    var vy = (fy - 0.5f) * lenY;

                    for (int x = 0; x < maxX + 1; x++)
                    {
                        indexes[y][x] = vertices.Count;
                        var r2 = Mathf.Deg2Rad * x * 360 / maxX;
                        var v = new Vector3(Mathf.Cos(r2) * lenX, vy, Mathf.Sin(r2) * lenX);
                        //vertices.Add(v);
                        vertices.Add(Vector3.Scale(v, sc));
                        //normals.Add((v - Vector3.up * vy).normalized);
                        normals.Add(Vector3.Scale((v - Vector3.up * vy), new(sc.y * sc.z, sc.z * sc.x, sc.x * sc.y)).normalized);
                        //uv.Add(new(x / (float)maxX, fy));
                        uv.Add(new(x / (float)maxX, v.y / len + 0.5f));
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
