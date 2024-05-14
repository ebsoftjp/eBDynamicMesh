#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace eBDynamicMesh.Editor
{
    public class Export
    {
        public Export()
        {
        }

        public void Exec()
        {
            // create data
            Factory.CreateCylinder(Paths.DataPath, 30, 10, 1f, 0.5f, false);
            var mesh = Factory.Get(Paths.DataPath);
            WriteFile(mesh, Paths.DataFullPath);
        }

        private void CreateDir(string path)
        {
            var dir = Regex.Replace(path, @"[^/]+?$", "");
            if (Directory.Exists(dir)) return;
            Directory.CreateDirectory(dir);
        }

        private void WriteFile(Mesh mesh, string path)
        {
            CreateDir(path);
            AssetDatabase.CreateAsset(mesh, path);
            EditorUtility.SetDirty(mesh);
        }
    }
}
#endif
