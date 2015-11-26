using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using TruGUI.Controls;
using TruGUI.Enums;
using UniRx;
using UnityEditor;

/// <summary>
/// The NuGet Editor Window. This is the class that handles all of the input for the package manager.
/// </summary>
public class NuGetWindow : EditorWindow
{
    string packageName;
    private NuGetViewModel _viewModel;
    private bool showPackages = false;
    private Vector2 packagesScrollPos;

    Button _installButton = new Button("Install Package")
    {
        IsLayout = true
    };
    Label _installPackageLabel = new Label(new GUIElementOptions())
    {
        IsLayout = true
    };

    Group _packagesGroup = new Group(GroupSortingType.Vertical)
    {
        IsLayout = true
    };

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
        nuget.titleContent = new GUIContent("NuGet");
        nuget.Show();
    }

    public NuGetWindow()
    {
        ViewModel.InstallMessage.BindTo(this, (v, m) => v._installPackageLabel.Text = m);
        _installButton.BindCommand(ViewModel, vm => vm.InstallPackage());
        ViewModel.Project.Dependencies.ObserveCountChanged().Subscribe(c => SetPackagesElements());
        SetPackagesElements();
    }

    private void SetPackagesElements()
    {
        _packagesGroup.Elements = ViewModel.Project.Dependencies.Select(d =>
        {
            var removeDependencyButton = new Button("X", string.Format("Remove {0}", d.Key))
            {
                Color = Color.red
            };
            removeDependencyButton.BindCommand(ViewModel, vm => vm.UninstallPackage(d.Key));

            return (TruGUIElement) new Group(GroupSortingType.Horizontal)
            {
                Elements = new TruGUIElement[]
                {
                    new Label(new GUIElementOptions())
                    {
                        Text = string.Format("{0}@{1}", d.Key, d.Value.Version)
                    },
                    removeDependencyButton
                }
            };
        }).ToArray();
    }

    private static NuGetViewModel InitViewModel()
    {
        return new NuGetViewModel(new Preferences());
    }

    void OnGUI()
    {
        var originalColor = GUI.backgroundColor;
        packageName = EditorGUILayout.TextField("Package", packageName);
        _installButton.Draw();
        _installPackageLabel.Draw();
        _packagesGroup.IsVisible = EditorGUILayout.Foldout(_packagesGroup.IsVisible, "Installed Packages");
        _packagesGroup.Draw();
        //if (showPackages)
        //{
        //    EditorGUILayout.BeginVertical(GUI.skin.box);
        //    var deps = ViewModel.Project.Dependencies.ToDictionary(d => d.Key, d => d.Value); // Copy the dependencies so that they are not modified while looping
        //    if (deps.Count == 0)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("There are no installed NuGet packages");
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    foreach (var package in deps)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.PrefixLabel(string.Format("{0}@{1}", package.Key, package.Value.Version));
        //        GUI.backgroundColor = Color.red;
        //        if (GUILayout.Button(new GUIContent("X", "Remove Dependency"), GUILayout.MaxWidth(25)))
        //        {
        //            ViewModel.UninstallPackage(package.Key);
        //        }
        //        GUI.backgroundColor = originalColor;
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    EditorGUILayout.EndVertical();
        //}
    }
}
