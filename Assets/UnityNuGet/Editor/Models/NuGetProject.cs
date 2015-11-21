using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using Debug = UnityEngine.Debug;

/// <summary>
/// Defines a JSON model that mimics the NuGet Project.json format.
/// This file attempts to use the same format that the NuGet Project.json file uses, 
/// but is not explicitly required due to the fact that the NuGet command line is being used to install packages.
/// </summary>
/// <seealso cref="https://docs.nuget.org/consume/projectjson-format"/>
public class NuGetProject
{
    /// <summary>
    /// Gets the NuGet dependencies that belong to this project.
    /// </summary>
    [JsonProperty(PropertyName = "dependencies")]
    private Dictionary<string, string> _rawDependencies
    {
        get { return BuiltDependencies.ToDictionary(d => d.Key, d => d.Value.Version); }
        set
        {
            BuiltDependencies = value == null ? null : value.ToDictionary(d => d.Key, d => new NuGetDependency(d));
        }
    }

    [JsonIgnore]
    public Dictionary<string, NuGetDependency> BuiltDependencies { get; private set; }

    [JsonProperty("packages_directory")]
    public string PackagesDirectory { get; set; }

    [JsonProperty(PropertyName = "runtimes")]
    private Dictionary<string, object> _rawRuntimes = new Dictionary<string, object>();

    public static NuGetProject LoadFromFile(string fileLocation)
    {
        using (StreamReader reader = new StreamReader(File.OpenRead(fileLocation)))
        {
            var project = JsonConvert.DeserializeObject<NuGetProject>(reader.ReadToEnd());
            return project;
        }
    }

    public static void SaveToFile(string fileLocation, NuGetProject project)
    {
        using (StreamWriter writer = new StreamWriter(fileLocation))
        {
            var s = JsonConvert.SerializeObject(project);

        }
    }

    public NuGetProject(string packagesLocation)
    {
        PackagesDirectory = packagesLocation;
        BuiltDependencies = new Dictionary<string, NuGetDependency>();
    }

    [Obsolete("Use NugetProject(string)")]
    public NuGetProject()
    {
        
    }

    public bool InstallDependency(string dependency, string outputDirectory = null, string version = null, bool? prerelease = null, bool? noCash = null, bool? requireConsent = null, string solutionDirectory = null)
    {
        return NuGetCommand(string.Format("install {0}", dependency), new
        {
            o = outputDirectory,
            v = version
        });
    }

    private bool NuGetCommand(string command, object options = null)
    {
        StringBuilder s = new StringBuilder(command);
        if (options != null)
        {
            s.AppendFormat(" {0}", string.Join(" ", options
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Select(p => new {name = p.Name, val = p.GetValue(options, null) })
                .Where(p => p.val != null)
                .Select(p => string.Format("-{0} {1}", p.name, p.val)).ToArray()));
        }
        Debug.Log(string.Format("NuGetCommand: '{0}'", s));
        string workingDirectory = Application.dataPath;
        var process = Process.Start(new ProcessStartInfo(Util.CombinePaths(workingDirectory, "Editor", "Lib", "NuGet", "nuget.exe"), s.ToString())
        {
            CreateNoWindow  = true,
            WorkingDirectory = PackagesDirectory
        });
        return process != null && process.WaitForExit(20000) && process.ExitCode == 0;
    }
}
