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
        };

        eBDynamicMesh.Factory.CreatePlane(names[0], new Vector3(1, 2, 0));
        eBDynamicMesh.Factory.CreateSphere(names[1], 10, 10, 0.5f);
        eBDynamicMesh.Factory.CreateFan(names[2], new() { new() });
        eBDynamicMesh.Factory.CreateCylinder(names[3], 10, 10, 2, 0.5f);
        eBDynamicMesh.Factory.CreateLine(names[4], new() { new() });

        for (int i = 0; i <names.Length; i++)
        {
            var obj = eBDynamicMesh.Factory.GetWithGameObject(names[i], material);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.right * (float)(i * 2 - (names.Length - 1)) * 0.5f;
        }
    }
}
