using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEditor;
using Debug = UnityEngine.Debug;

/// <summary>
/// The NuGet Editor Window. This is the class that handles all of the input for the package manager.
/// </summary>
public class NuGetWindow : EditorWindow
{
    string _installResult;
    private NuGetViewModel _viewModel;
    private bool _showPackages = false;
    private Vector2 _packagesScrollPos;
    private bool _isMissingDependency = false;
    private Stopwatch _missingDependenciesTimer;

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

    void OnEnable()
    {
        if (_missingDependenciesTimer == null)
        {
            _missingDependenciesTimer = new Stopwatch();
        }
    }

    void OnDisable()
    {
        if (_missingDependenciesTimer != null)
        {
            _missingDependenciesTimer.Stop();
            _missingDependenciesTimer = null;
        }
    }

    private static NuGetViewModel InitViewModel()
    {
        return new NuGetViewModel(new Preferences());
    }

    void Update()
    {
        if (_missingDependenciesTimer.ElapsedMilliseconds % 1000 == 0)
        {
            _isMissingDependency = ViewModel.IsMissingDependency();
        }
    }

    void OnGUI()
    {
        var originalColor = GUI.backgroundColor;
        ViewModel.NewPackageName = EditorGUILayout.TextField("Package", ViewModel.NewPackageName);
        ViewModel.NewPackageVersion = EditorGUILayout.TextField("Version", ViewModel.NewPackageVersion);
        if (GUILayout.Button("Install Package"))
        {
            if (ViewModel.InstallPackage())
            {
                _installResult = string.Format("Installed {0}!", ViewModel.NewPackageName);
                ViewModel.NewPackageName = "";
                ViewModel.NewPackageVersion = "";
            }
            else
            {
                _installResult = "Install Failed";
            }
        }
        if (_installResult != null)
        {
            EditorGUILayout.SelectableLabel(_installResult);
        }
        _showPackages = EditorGUILayout.Foldout(_showPackages, "Packages");
        if (_showPackages)
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

        GUI.enabled = _isMissingDependency;
        if (GUILayout.Button(_isMissingDependency ? "Restore Missing Packages" : "All Packages are Installed!"))
        {
            ViewModel.RestoreMissingPackages();
            _isMissingDependency = false;
        }
        GUI.enabled = true;
    }
}
