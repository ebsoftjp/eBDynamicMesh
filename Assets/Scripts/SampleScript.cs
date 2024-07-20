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
            "SkCircle2",
            "SkCircle3",
            "SkCircle4",
            "SkLine",
            "SkLine2",
            "SkLine3",
        };

        var n = 0;
        eBDynamicMesh.Factory.CreatePlane(names[n++], new Vector3(1, 2, 0));
        eBDynamicMesh.Factory.CreateSphere(names[n++], 10, 10, 0.5f);
        eBDynamicMesh.Factory.CreateFan(names[n++], new() { new() });
        eBDynamicMesh.Factory.CreateCylinder(names[n++], 10, 10, 2, 0.5f);
        eBDynamicMesh.Factory.CreateLine(names[n++], new() { new() });
        eBDynamicMesh.Factory.CreateSphere(names[n++], 100, 100, 0.5f, new(0.5f, 1, 0.1f));
        eBDynamicMesh.Factory.CreateCapsule(names[n++], 20, 20, 20, 0.5f, 2f);
        eBDynamicMesh.Factory.CreateCapsule(names[n++], 20, 20, 20, 0.5f, 2f, new(0.5f, 1, 0.1f));
        //eBDynamicMesh.Factory.CreateSkinnedCircle(names[n++], new() { new() });
        //eBDynamicMesh.Factory.CreateSkinnedLine(names[n++], new());

        var ssphere1 = eBDynamicMesh.Factory
            .CreateWork()
            .SetScale(1, 1, 0.2f)
            .AddBone(6, 1.2f)
            .AddSphere(0, 0.25f, 0, 0.5f)
            //.AddLine(0.25f, 0.5f)
            //.AddLine(0.5f, 0.75f)
            .AddSphere(0.25f, 0.5f, 0.5f, 0.6f)
            .AddSphere(0.5f, 0.75f, 0.4f, 0.5f)
            .AddSphere(0.75f, 1, 0.5f, 0.75f)
            .ToMesh();
        eBDynamicMesh.Factory.Add(names[n++], ssphere1);

        eBDynamicMesh.Factory.Add(names[n++], eBDynamicMesh.Factory
            .CreateWork()
            .AddBone(6, 1.2f)
            .AddSphere(0, 1, 0, 1)
            .ToMesh());

        eBDynamicMesh.Factory.Add(names[n++], eBDynamicMesh.Factory
            .CreateWork()
            .AddBone(6, 1.2f)
            .AddSphere(0.5f, 1, 0, 0.1f)
            .ToMesh());

        eBDynamicMesh.Factory.Add(names[n++], eBDynamicMesh.Factory
            .CreateWork()
            .AddBone(6, 1.2f)
            .AddSphere(0.5f, 1, 0.1f, 0.2f)
            .ToMesh());

        var sline1 = eBDynamicMesh.Factory
            .CreateWork()
            .AddBone(6, 1.2f)
            .AddCorn(0, 0.25f, 0.5f, 1)
            .AddLine(0.25f, 0.75f)
            .SetColor(Color.green)
            .AddCorn(0.75f, 1, 1, 0)
            .ToMesh();
        eBDynamicMesh.Factory.Add(names[n++], sline1);
        eBDynamicMesh.Factory.Add(names[n++], sline1);

        var sline3 = eBDynamicMesh.Factory
            .CreateWork()
            .SetLength(0.1f, 1)
            .AddBone(6, 1.2f)
            .SetColor(Color.green)
            .AddCorn(0, 0.05f, 0, 1)
            .AddLine(0.05f, 0.7f)
            .AddCorn(0.75f, 1, 2, 0)
            .AddCorn(0.7f, 0.75f, 1, 2)
            .ToMesh();
        eBDynamicMesh.Factory.Add(names[n++], sline3);

        var maxX = 3;
        for (int i = 0; i < names.Length; i++)
        {
            var x = i % maxX;
            var y = i / maxX;
            var obj = names[i].StartsWith("Sk")
                ? eBDynamicMesh.Factory.GetWithSkinnedObject(names[i], material, 6)
                : eBDynamicMesh.Factory.GetWithGameObject(names[i], material);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new((float)(x * 2 - (maxX - 1)) * 1f, y * 2f - 2, 0);

            if (i == 12) a(obj);
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
