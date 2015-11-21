using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NuGetDependency
{
    /// <summary>
    /// Gets or sets the version for this dependency.
    /// </summary>
    public string Version { get; set; }

    public NuGetDependency(KeyValuePair<string, string> dependencyNameAndVersion)
    {
        Version = dependencyNameAndVersion.Value;
    }
}
