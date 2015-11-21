using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// The NuGet Editor Window. This is the class that handles all of the input for the package manager.
/// </summary>
public class NuGetWindow : EditorWindow
{
    string packageName;
    string installResult;
    NuGetViewModel viewModel;

    /// <summary>
    /// Opens the NuGet Editor Window
    /// </summary>
    [MenuItem("Window/NuGet")]
    public static void Init()
    {
        NuGetWindow nuget = EditorWindow.GetWindow<NuGetWindow>();
        nuget.viewModel = new NuGetViewModel(new Preferences());
        nuget.Show();
    }

    void OnGUI()
    {
        packageName = EditorGUILayout.TextField("Package", packageName);
        if (GUILayout.Button("Install Package"))
        {
            if (viewModel.InstallPackage(packageName))
            {
                installResult = "Installed!";
            }
        }
        if (installResult != null)
        {
            EditorGUILayout.SelectableLabel(installResult);
        }
    }
}
