using System;
using UnityEngine;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using UniRx;
using UnityEditor;

/// <summary>
/// Defines a class that represents a view model for the <see cref="NuGetWindow"/> Editor Window.
/// </summary>
public class NuGetViewModel : ViewModelBase
{
    public const string DefaultPackagesDirectory = "Packages";

    /// <summary>
    /// Gets the Path to the location that the NuGet project.json file is at.
    /// </summary>
    public ReactiveProperty<string> PackagesJsonLocation { get; private set; }

    /// <summary>
    /// Gets the project that this view model houses.
    /// </summary>
    public NuGetProject Project { get; private set; }

    public IPreferences Preferences { get; private set; }

    public ReactiveProperty<string> InstallMessage { get; private set; }

    public ReactiveProperty<string> Package { get; private set; }

    public NuGetViewModel([NotNull] IPreferences preferences)
    {
        if (preferences == null) throw new ArgumentNullException("preferences");
        Preferences = preferences;
        PackagesJsonLocation = new ReactiveProperty<string>(Preferences.GetString("PackagesJsonLocation", Util.CombinePaths(Application.dataPath, "project.json")));
        Debug.Log(string.Format("Load Project from File: '{0}'", PackagesJsonLocation));
        Project = NuGetProject.LoadFromFile(PackagesJsonLocation.Value, DefaultPackagesDirectory);
        InstallMessage = new ReactiveProperty<string>();
    }

    /// <summary>
    /// Installs the given package into the project and saves the project.
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    public void InstallPackage()
    {
        var installResult = Project.InstallDependency(Package.Value);
        if (installResult.Succeeded)
        {
            switch (installResult.Result)
            {
                case InstallResult.InstallSucceeded:
                    Project.Save();
                    InstallMessage.Value = string.Format("Installed {0}!", Package.Value);
                    break;
                case InstallResult.InstallSucceededButNoSupportedFrameworks:
                    Project.UninstallDependency(Package.Value);
                    Project.Save();
                    InstallMessage.Value = "The Package was Found, but it did not contain any library for .Net 3.5";
                    break;
            }
        }
        else
        {
            InstallMessage.Value = "The Package Could Not Be Installed.";
        }
    }

    public void UninstallPackage(string package)
    {
        Project.UninstallDependency(package);
        Project.Save();
    }
}
