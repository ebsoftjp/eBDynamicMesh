using System.Linq;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    void Start()
    {
        var material = new Material(Resources.Load<Material>("Materials/Default"));

        var names = new string[]
        {
            "Plane",
            "Sphere",
            "Fan",
            "Cylinder",
            "Line",
            "Sphere2",
            "Capsule",
            "Capsule2",
            "SkCircle",
            "SkLine",
            "SkLine2",
        };

        eBDynamicMesh.Factory.CreatePlane(names[0], new Vector3(1, 2, 0));
        eBDynamicMesh.Factory.CreateSphere(names[1], 10, 10, 0.5f);
        eBDynamicMesh.Factory.CreateFan(names[2], new() { new() });
        eBDynamicMesh.Factory.CreateCylinder(names[3], 10, 10, 2, 0.5f);
        eBDynamicMesh.Factory.CreateLine(names[4], new() { new() });
        eBDynamicMesh.Factory.CreateSphere(names[5], 100, 100, 0.5f, new(0.5f, 1, 0.1f));
        eBDynamicMesh.Factory.CreateCapsule(names[6], 20, 20, 20, 0.5f, 2f);
        eBDynamicMesh.Factory.CreateCapsule(names[7], 20, 20, 20, 0.5f, 2f, new(0.5f, 1, 0.1f));
        eBDynamicMesh.Factory.CreateSkinnedCircle(names[8], new() { new() });
        //eBDynamicMesh.Factory.CreateSkinnedLine(names[9], new());

        var mesh9 = eBDynamicMesh.Factory
            .CreateWork()
            .AddBone(6, 1.2f)
            .AddCorn(0.5f, 1, 0, 0.25f)
            .AddLine(0.25f, 0.75f)
            .AddCorn(1, 0, 0.75f, 1)
            .ToMesh();
        eBDynamicMesh.Factory.Add(names[9], mesh9);
        eBDynamicMesh.Factory.Add(names[10], mesh9);

        var maxX = 3;
        for (int i = 0; i < names.Length; i++)
        {
            var x = i % maxX;
            var y = i / maxX;
            var obj = names[i].StartsWith("Sk")
                ? eBDynamicMesh.Factory.GetWithSkinnedObject(names[i], material, 6)
                : eBDynamicMesh.Factory.GetWithGameObject(names[i], material);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new((float)(x * 2 - (maxX - 1)) * 1f, y * 2f - 1, 0);

            if (i == 9) a(obj);
        }
    }

    private static void a(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var f = (float)i / (obj.transform.childCount - 1);
            var r = f * 2 * Mathf.PI;
            //var m = Matrix4x4.identity;
            var l2 = 0.5f;
            var pos = new Vector3(Mathf.Cos(r) * l2, Mathf.Sin(r) * l2, 0);
            //var pos = Vector3.zero;

            var t = obj.transform.GetChild(i);
            t.SetParent(obj.transform);
            t.SetLocalPositionAndRotation(pos, Quaternion.Euler(0, 0, f * 360));
            t.localScale = Vector3.one;
        }
    }
}
