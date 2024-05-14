namespace eBDynamicMesh.Editor
{
    public static class Paths
    {
        public const string NameSpace = "DynamicMesh";
        public static string BasePath = $"Assets/{NameSpace}";
        public const string OpenWindowPath = "Window/eB Dynamic Mesh/Export";

        public static string DataPath => $"Mesh";
        public static string DataFullPath => $"{BasePath}/Resources/{DataPath}.asset";
    }
}
