using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public static class Line
    {
        public static Work Add(Work work, int maxX, int maxY, float lenX1, float lenX2, float lenY, float per1, float per2)
        {
            var indexes = new int[maxY + 1][];
            var dy = (lenX1 - lenX2) / (lenY / (per2 - per1));

            // loop
            for (int y = 0; y < maxY + 1; y++)
            {
                // center
                var fy = (float)y / maxY;
                var per = Mathf.Lerp(per1, per2, fy);
                var vy = (per - 0.5f) * lenY;

                indexes[y] = new int[maxX + 1];
                var lenX = Mathf.Lerp(lenX1, lenX2, fy);
                for (int x = 0; x < maxX + 1; x++)
                {
                    indexes[y][x] = work.vertices.Count;
                    var r2 = Mathf.Deg2Rad * x * 360 / maxX;
                    var v1 = new Vector3(Mathf.Cos(r2), 0, Mathf.Sin(r2));
                    var v2 = new Vector3(v1.x * lenX / 2, vy, v1.z * lenX / 2);
                    work.vertices.Add(v2);
                    v1.y += dy;
                    work.normals.Add(v1.normalized);
                    work.uv.Add(new(x / (float)maxX, per));
                    work.colors.Add(work.color);
                }

                // bone
                work.AddWeight(work.vertices[^1].y, maxX + 1);
            }

            for (int y = 0; y < maxY; y++)
            {
                var y1 = per1 < per2 ? y : y + 1;
                var y2 = per1 < per2 ? y + 1 : y;
                for (int x = 0; x < maxX; x++)
                {
                    var x2 = x + 1;
                    var n1 = indexes[y1][x];
                    var n2 = indexes[y1][x2];
                    var n3 = indexes[y2][x];
                    var n4 = indexes[y2][x2];
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
