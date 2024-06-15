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
        };

        eBDynamicMesh.Factory.CreatePlane(names[0], new Vector3(1, 2, 0));
        eBDynamicMesh.Factory.CreateSphere(names[1], 10, 10, 0.5f);
        eBDynamicMesh.Factory.CreateFan(names[2], new() { new() });
        eBDynamicMesh.Factory.CreateCylinder(names[3], 10, 10, 2, 0.5f);
        eBDynamicMesh.Factory.CreateLine(names[4], new() { new() });
        eBDynamicMesh.Factory.CreateSphere(names[5], 100, 100, 0.5f, new(0.5f, 1, 0.1f));

        var maxX = 3;
        for (int i = 0; i < names.Length; i++)
        {
            var x = i % maxX;
            var y = i / maxX;
            var obj = eBDynamicMesh.Factory.GetWithGameObject(names[i], material);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new((float)(x * 2 - (maxX - 1)) * 1f, y * 3f - 1, 0);
        }
    }
}
