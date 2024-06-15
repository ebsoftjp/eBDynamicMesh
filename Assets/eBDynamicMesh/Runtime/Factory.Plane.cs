using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public partial class Factory
    {
        public static Mesh CreatePlane(string name, Vector3 len)
        {
            var uvTable = new Vector2[]
            {
                Vector2.zero,
                Vector2.up,
                Vector2.right,
                Vector2.one,
            };

            var vertices = new List<Vector3>();
            var uv = new List<Vector2>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();
            var colors = new List<Color>();

            // vertices
            for (int i = 0; i < 4; i++)
            {
                var p1 = (float)(i / 2) - 0.5f;
                var p2 = (float)(i % 2) - 0.5f;
                vertices.Add(len.x == 0
                    ? new(0, p1 * len.y, p2 * len.z)
                    : (len.y == 0 ? new(p1 * len.x, 0, p2 * len.z) : new(p1 * len.x, p2 * len.y, 0)));
                uv.Add(uvTable[i]);
                normals.Add(len.x == 0
                    ? Vector3.right
                    : (len.y == 0 ? Vector3.up : Vector3.back));
                colors.Add(Color.white);
            }

            // triangles
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(3);
            triangles.Add(2);

            // mesh
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
