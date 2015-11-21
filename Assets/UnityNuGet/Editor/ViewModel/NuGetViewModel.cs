using System;
using UnityEngine;
using System.Collections;
using System.IO;
using JetBrains.Annotations;

/// <summary>
/// Defines a class that represents a view model for the <see cref="NuGetWindow"/> Editor Window.
/// </summary>
public class NuGetViewModel : ViewModelBase
{
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
        PackagesJsonLocation = Preferences.GetString("PackagesJsonLocation");
        if (!string.IsNullOrEmpty(PackagesJsonLocation))
        {
            Debug.Log(string.Format("Load Project from File: '{0}'", PackagesJsonLocation));
            Project = NuGetProject.LoadFromFile(PackagesJsonLocation);
        }
        else
        {
            Project = new NuGetProject(Util.CombinePaths(Application.dataPath, "Packages"));
        }
    }

    public bool InstallPackage(string package)
    {
        return Project.InstallDependency(package);
    }
}
