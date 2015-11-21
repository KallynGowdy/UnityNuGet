using System;
using UnityEngine;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
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
    public string PackagesJsonLocation { get; private set; }

    /// <summary>
    /// Gets the project that this view model houses.
    /// </summary>
    public NuGetProject Project { get; private set; }

    public IPreferences Preferences { get; private set; }

    public NuGetViewModel([NotNull] IPreferences preferences)
    {
        if (preferences == null) throw new ArgumentNullException("preferences");
        Preferences = preferences;
        PackagesJsonLocation = Preferences.GetString("PackagesJsonLocation", Util.CombinePaths(Application.dataPath, "project.json"));
        Debug.Log(string.Format("Load Project from File: '{0}'", PackagesJsonLocation));
        Project = NuGetProject.LoadFromFile(PackagesJsonLocation, DefaultPackagesDirectory);
    }

    /// <summary>
    /// Installs the given package into the project and saves the project.
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    public bool InstallPackage(string package)
    {
        var installResult = Project.InstallDependency(package);
        switch (installResult)
        {
            case InstallResult.InstallSucceeded:
                Project.Save();
                return true;
            case InstallResult.InstallSucceededButNoSupportedFrameworks:
                Project.UninstallDependency(package);
                Project.Save();
                return false;
            default:
                return false;
        }
    }

    public void UninstallPackage(string package)
    {
        Project.UninstallDependency(package);
        Project.Save();
    }
}
