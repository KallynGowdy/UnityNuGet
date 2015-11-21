using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public static class Util
{
    public static string CombinePaths(params string[] paths)
    {
        return paths.Aggregate<string, string>(null, (current, path) => current != null ? Path.Combine(current, path) : path);
    }

    public static string BuildAssetsDirectory(string relativePath)
    {
        return CombinePaths(Application.dataPath, relativePath);
    }
}
