using UnityEngine;
using System.Collections;
using System.Linq;
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
        var originalColor = GUI.backgroundColor;
        packageName = EditorGUILayout.TextField("Package", packageName);
        if (GUILayout.Button("Install Package"))
        {
            if (ViewModel.InstallPackage(packageName))
            {
                installResult = string.Format("Installed {0}!", packageName);
                packageName = "";
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
            EditorGUILayout.BeginVertical(GUI.skin.box);
            var deps = ViewModel.Project.Dependencies.ToDictionary(d => d.Key, d => d.Value);
            if (deps.Count == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("There are no installed NuGet packages");
                EditorGUILayout.EndHorizontal();
            }
            foreach (var package in deps)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(string.Format("{0}@{1}", package.Key, package.Value.Version));
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button(new GUIContent("X", "Remove Dependency"), GUILayout.MaxWidth(25)))
                {
                    ViewModel.UninstallPackage(package.Key);
                }
                GUI.backgroundColor = originalColor;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
