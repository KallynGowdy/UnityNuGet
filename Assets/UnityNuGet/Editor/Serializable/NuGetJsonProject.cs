using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// Defines a class that represents a JSON representation of a <see cref="NuGetJsonProject"/>.
/// Mostly an Implementation Detail.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class NuGetJsonProject
{
    public NuGetJsonProject()
    {
    }

    public NuGetJsonProject(NuGetProject project)
    {
        Dependencies = project.RawDependencies;
        PackagesDirectory = project.PackagesDirectory;
        Frameworks = project.Frameworks.ToDictionary(f => f.Key, f => f.Value);
    }

    [JsonProperty("frameworks")]
    public Dictionary<string, object> Frameworks { get; set; }

    [JsonProperty("packages_directory")]
    public string PackagesDirectory { get; set; }

    [JsonProperty("dependencies")]
    public Dictionary<string, string> Dependencies { get; set; }
}
