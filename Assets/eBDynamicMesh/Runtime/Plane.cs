using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public static class Plane
    {
        public static Work Add(Work work, int maxX, int maxY, float lenX, float lenY)
        {
            return AddOne(work, Vector3.zero, Vector3.up, work.scale, maxX, maxY, lenX, lenY, Matrix4x4.identity);
        }

        public static Work AddOne(Work work, Vector3 basePos, Vector3 normal, Vector3 sc, int maxX, int maxY, float lenX, float lenY, Matrix4x4 m)
        {
            var count = work.vertices.Count;

            // vertices
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    var t1 = (float)x / (float)(maxX - 1);
                    var t2 = (float)y / (float)(maxY - 1);
                    var p1 = t1 - 0.5f;
                    var p2 = t2 - 0.5f;
                    work.vertices.Add(basePos + Vector3.Scale(m.MultiplyPoint3x4(new(p1 * lenX, 0, p2 * lenY)), sc));
                    work.uv.Add(new(t1, t2));
                    work.normals.Add(normal);
                    work.colors.Add(work.color);
                }
            }

            // triangles
            for (int y = 0; y < maxY - 1; y++)
            {
                for (int x = 0; x < maxX - 1; x++)
                {
                    var n1 = count + x + y * maxX;
                    var n2 = n1 + maxX;
                    work.triangles.Add(n1 + 0);
                    work.triangles.Add(n2 + 0);
                    work.triangles.Add(n1 + 1);
                    work.triangles.Add(n1 + 1);
                    work.triangles.Add(n2 + 0);
                    work.triangles.Add(n2 + 1);
                }
            }

            return work;
        }
    }
}
