#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace eBDynamicMesh.Editor
{
    class Window : EditorWindow
    {
        [MenuItem(Paths.OpenWindowPath)]
        public static void Open()
        {
            GetWindowWithRect<Window>(new Rect(0, 0, 320, 240), true, "eB Dynamic Mesh - Settings", true);
        }

        private void OnGUI()
        {
            // header
            EditorGUILayout.BeginVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("eB Dynamic Mesh control panel");
            var rect = new Rect(40.0f, 60.0f, 240.0f, 20.0f);
            var cv = new Export();
            var addy = 32f;

            if (GUI.Button(rect, "Export"))
            {
                cv.Exec();
            }

            rect.y += addy;
            if (GUI.Button(rect, "Open export asset"))
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Mesh>(Paths.DataFullPath);
            }

            rect.y += addy;
            if (GUI.Button(rect, "Close"))
            {
                Close();
            }
        }
    }
}
#endif
