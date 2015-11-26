using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NuGetDependency
{
    /// <summary>
    /// Gets or sets the version for this dependency.
    /// </summary>
    public string Version { get; set; }

    public string Name { get; private set; }

    public string InstallLocation { get; private set; }

    public string LibPath
    {
        get { return Util.CombinePaths(InstallLocation, "lib"); }
    }

    public NuGetDependency(string dependencyVersion, string dependencyName, string packagesLocation)
    {
        Version = dependencyVersion;
        Name = dependencyName;
        InstallLocation = Util.CombinePaths(packagesLocation, string.Format("{0}.{1}", dependencyName, dependencyVersion));
    }

    /// <summary>
    /// Removes all of the unsupported frameworks from the lib folder for the dependency and returns whether any supported frameworks exist for the dependency.
    /// </summary>
    /// <param name="supportedFrameworks"></param>
    /// <returns></returns>
    public bool RemoveUnsupportedFrameworks(Dictionary<string, object> supportedFrameworks)
    {
        var frameworks = Directory.GetDirectories(LibPath).ToList();
        foreach (var unsupportedFramework in frameworks.Where(f => supportedFrameworks.All(sf => !f.EndsWith(sf.Key))).ToArray())
        {
            frameworks.Remove(unsupportedFramework);
            Directory.Delete(unsupportedFramework, true);
        }
        return frameworks.Count > 0;
    }

    /// <summary>
    /// Uninstalls this dependency from the file system.
    /// </summary>
    /// <returns></returns>
    public bool Uninstall()
    {
        if (IsInstalled())
        {
            Directory.Delete(InstallLocation, true);
        }
        return true;
    }

    /// <summary>
    /// Gets whether this dependency exists on disk.
    /// </summary>
    /// <returns></returns>
    public bool IsInstalled()
    {
        return Directory.Exists(InstallLocation);
    }
}
