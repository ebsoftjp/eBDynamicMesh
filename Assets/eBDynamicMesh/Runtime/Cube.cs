using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace eBDynamicMesh
{
    public static class Cube
    {
        public static Work Add(Work work, int maxX, int maxY, float lenX, float lenY, float per1, float per2)
        {
            var bpos = Vector3.Lerp(Vector3.down * lenY / 2, Vector3.up * lenY / 2, (per1 + per2) / 2);
            lenY *= per2 - per1;
            var p = Vector3.zero;
            var s = Vector3.one;
            Plane.AddOne(work, bpos + Vector3.forward * lenX / 2 * work.scale.z, Vector3.forward,
                work.scale, maxX, maxY, lenX, lenY, Matrix4x4.TRS(p, Quaternion.Euler(-90, 0, 180), s));
            Plane.AddOne(work, bpos + Vector3.back * lenX / 2 * work.scale.z, Vector3.back,
                work.scale, maxX, maxY, lenX, lenY, Matrix4x4.TRS(p, Quaternion.Euler(-90, 180, 180), s));
            Plane.AddOne(work, bpos + Vector3.left * lenX / 2 * work.scale.x, Vector3.left,
                work.scale, maxX, maxY, lenX, lenY, Matrix4x4.TRS(p, Quaternion.Euler(-90, 270, 180), s));
            Plane.AddOne(work, bpos + Vector3.right * lenX / 2 * work.scale.x, Vector3.right,
                work.scale, maxX, maxY, lenX, lenY, Matrix4x4.TRS(p, Quaternion.Euler(-90, 90, 180), s));
            return work;
        }
    }
}
