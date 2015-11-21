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
    private NuGetViewModel _viewModel;
    private bool showPackages = false;
    private Vector2 packagesScrollPos;

    NuGetViewModel ViewModel
    {
        get { return _viewModel ?? (_viewModel = InitViewModel()); }
        set { _viewModel = value; }
    }

    /// <summary>
    /// Opens the NuGet Editor Window
    /// </summary>
    [MenuItem("Window/NuGet")]
    public static void Init()
    {
        NuGetWindow nuget = EditorWindow.GetWindow<NuGetWindow>();
        nuget.Show();
    }

    private static NuGetViewModel InitViewModel()
    {
        return new NuGetViewModel(new Preferences());
    }

    void OnGUI()
    {
        packageName = EditorGUILayout.TextField("Package", packageName);
        if (GUILayout.Button("Install Package"))
        {
            if (ViewModel.InstallPackage(packageName))
            {
                installResult = string.Format("Installed {0}!", packageName);
            }
            else
            {
                installResult = "Install Failed";
            }
        }
        if (installResult != null)
        {
            EditorGUILayout.SelectableLabel(installResult);
        }
        showPackages = EditorGUILayout.Foldout(showPackages, "Packages");
        if (showPackages)
        {

            foreach (var package in ViewModel.Project.Dependencies)
            {
                EditorGUILayout.PrefixLabel(package.Key);
                if (GUILayout.Button("X"))
                {
                    ViewModel.UninstallPackage(package.Key);
                }
            }
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();

        }
    }
}
