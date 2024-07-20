using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public static class Sphere
    {
        public static Work Add(Work work, int maxX, int maxY, float sp1, float sp2, float per1, float per2)
        {
            var indexes = new int[maxY][];

            var radius = work.lenX / 2;
            var ty0 = -work.lenY / 2;
            var ty1 = work.lenY / 2;
            var orgY1 = Mathf.Cos((1 - sp1) * 180 * Mathf.Deg2Rad);
            var orgY2 = Mathf.Cos((1 - sp2) * 180 * Mathf.Deg2Rad);

            // scale
            var sc = work.scale;
            sc.y *= work.lenY / work.lenX / ((orgY2 - orgY1) / 2) * (per2 - per1);

            // offset
            var ofsY = (per1 - 0.5f) * work.lenY - orgY1 * radius * sc.y;

            // loop
            for (int y = 0; y < maxY; y++)
            {
                // center
                var fy = (float)y / (float)(maxY - 1);
                var r1 = (1 - Mathf.Lerp(sp1, sp2, fy)) * 180 * Mathf.Deg2Rad;
                var vy = Mathf.Cos(r1) * radius;
                var l1 = Mathf.Sin(r1) * radius;
                //var maxX2 = (y == 0 || y == maxY - 1) ? 1 : maxX + 1;
                var maxX2 = maxX + 1;

                indexes[y] = new int[maxX2];
                for (int x = 0; x < maxX2; x++)
                {
                    indexes[y][x] = work.vertices.Count;
                    var r2 = x * 360 / maxX * Mathf.Deg2Rad;
                    var v = new Vector3(Mathf.Cos(r2) * l1, vy, Mathf.Sin(r2) * l1);
                    work.vertices.Add(Vector3.Scale(v, sc) + new Vector3(0, ofsY, 0));
                    //normals.Add(v.normalized);
                    //normals.Add(Vector3.Scale(v, sc).normalized);
                    work.normals.Add(Vector3.Scale(v, new(sc.y * sc.z, sc.z * sc.x, sc.x * sc.y)).normalized);
                    work.uv.Add(new Vector2((float)x / (float)maxX, Mathf.InverseLerp(ty0, ty1, work.vertices[^1].y)));
                    work.colors.Add(work.color);
                }

                // bone
                work.AddWeight(work.vertices[^1].y, maxX2);
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
                    //if (y > 0)
                    //{
                    //    work.triangles.Add(n1);
                    //    work.triangles.Add(n2);
                    //    work.triangles.Add(n3);
                    //}
                    //if (y < maxY - 2)
                    //{
                    //    work.triangles.Add(n2);
                    //    work.triangles.Add(n4);
                    //    work.triangles.Add(n3);
                    //}
                    work.triangles.Add(n1);
                    work.triangles.Add(n3);
                    work.triangles.Add(n2);
                    work.triangles.Add(n2);
                    work.triangles.Add(n3);
                    work.triangles.Add(n4);
                }
            }

            return work;
        }
    }
}
