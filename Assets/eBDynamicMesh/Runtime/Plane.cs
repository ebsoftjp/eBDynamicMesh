using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public static class Plane
    {
        public static Work Add(Work work, int maxX, int maxY, float len2)
        {
            var sc = work.scale;

            // vertices
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    var t1 = (float)x / (float)(maxX - 1);
                    var t2 = (float)y / (float)(maxY - 1);
                    var p1 = t1 - 0.5f;
                    var p2 = t2 - 0.5f;
                    work.vertices.Add(Vector3.Scale(new(p1, 0, p2), sc * len2));
                    work.uv.Add(new(t1, t2));
                    work.normals.Add(Vector3.up);
                    work.colors.Add(work.color);
                }
            }

            // triangles
            for (int y = 0; y < maxY - 1; y++)
            {
                for (int x = 0; x < maxX - 1; x++)
                {
                    var n1 = x + y * maxX;
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
