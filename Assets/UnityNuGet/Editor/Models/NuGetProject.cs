using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEditor;
using Debug = UnityEngine.Debug;

/// <summary>
/// Defines a JSON model that mimics the NuGet Project.json format.
/// This file attempts to use the same format that the NuGet Project.json file uses, 
/// but is not explicitly required due to the fact that the NuGet command line is being used to install packages.
/// </summary>
/// <seealso cref="https://docs.nuget.org/consume/projectjson-format"/>
[JsonObject(MemberSerialization.OptIn)]
public class NuGetProject
{
    private static readonly Regex findVersionRegex = new Regex(@"(?<version>\d+\.\d+\.\d+)", RegexOptions.Compiled);

    /// <summary>
    /// Gets the NuGet dependencies that belong to this project.
    /// </summary>
    [JsonProperty(PropertyName = "dependencies")]
    public Dictionary<string, string> RawDependencies
    {
        get { return Dependencies.ToDictionary(d => d.Key, d => d.Value.Version); }
        set
        {
            Dependencies = value == null ? null : value.ToDictionary(d => d.Key, d => new NuGetDependency(d.Value, d.Key, AbsolutePackagesDirectory));
        }
    }

    /// <summary>
    /// Gets the list of NuGet dependencies that the project has.
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, NuGetDependency> Dependencies
    {
        get { return _dependencies ?? (_dependencies = new Dictionary<string, NuGetDependency>()); }
        private set { _dependencies = value; }
    }

    /// <summary>
    /// Gets the list of frameworks that are supported by this project.
    /// </summary>
    [JsonProperty("frameworks")]
    public Dictionary<string, object> Frameworks
    {
        get
        {
            return _frameworks ??
                    (_frameworks = new Dictionary<string, object>()
                    {
                        {"net35", new object() }
                    });
        }
        private set { _frameworks = value; }
    }

    /// <summary>
    /// Gets or sets the directory that the packages are stored in.
    /// </summary>
    [JsonProperty("packages_directory")]
    public string PackagesDirectory
    {
        get
        {
            var absolutePath = Util.BuildAssetsDirectory(_packagesDirectory);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
            return _packagesDirectory;
        }
        set { _packagesDirectory = value ?? ""; }
    }

    /// <summary>
    /// Gets the Absolute Path to the Packages Directory.
    /// </summary>
    [JsonIgnore]
    public string AbsolutePackagesDirectory
    {
        get { return Util.BuildAssetsDirectory(PackagesDirectory); }
    }

    /// <summary>
    /// Gets the location that the project.json file was loaded from.
    /// </summary>
    [JsonIgnore]
    private string _projectJsonLocation;

    private string _packagesDirectory;
    private Dictionary<string, object> _frameworks;
    private Dictionary<string, NuGetDependency> _dependencies;

    /// <summary>
    /// Loads the <see cref="NuGetProject"/> from the given file location, or creates a new project at the location if none exists.
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <returns></returns>
    public static NuGetProject LoadFromFile(string fileLocation, string defaultPackagesDirectory = "")
    {
        if (File.Exists(fileLocation))
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(fileLocation)))
            {
                var json = JsonConvert.DeserializeObject<NuGetJsonProject>(reader.ReadToEnd());
                var project = new NuGetProject(json) { _projectJsonLocation = fileLocation };
                return project;
            }
        }
        else
        {
            var project = new NuGetProject(defaultPackagesDirectory);
            project._projectJsonLocation = fileLocation;
            project.Save();
            return project;
        }
    }

    /// <summary>
    /// Saves the given <see cref="NuGetProject"/> to the given <paramref name="fileLocation"/>.
    /// </summary>
    /// <param name="fileLocation"></param>
    /// <param name="project"></param>
    public static void SaveToFile(string fileLocation, NuGetProject project)
    {
        Debug.Log(string.Format("Saving NuGet project to '{0}'", fileLocation));
        using (StreamWriter writer = new StreamWriter(fileLocation))
        {
            var json = new NuGetJsonProject(project);
            var s = JsonConvert.SerializeObject(json, Formatting.Indented);
            writer.Write(s);
            writer.Flush();
        }
    }

    public NuGetProject(string packagesLocation) : this()
    {
        PackagesDirectory = packagesLocation;
    }

    public NuGetProject()
    {
        Dependencies = new Dictionary<string, NuGetDependency>();
        PackagesDirectory = "";
    }

    public NuGetProject([NotNull] NuGetJsonProject project)
    {
        if (project == null) throw new ArgumentNullException("project");
        PackagesDirectory = project.PackagesDirectory;
        RawDependencies = project.Dependencies;
        Frameworks = project.Frameworks;
    }

    /// <summary>
    /// Saves the project to file.
    /// </summary>
    public void Save()
    {
        SaveToFile(_projectJsonLocation, this);
    }

    public void RestoreDependencies()
    {
        foreach (var missingDep in GetMissingDependencies().ToArray())
        {
            InstallDependency(missingDep.Name, version: missingDep.Version);
        }
    }

    public IEnumerable<NuGetDependency> GetMissingDependencies()
    {
        return Dependencies.Where(d => !d.Value.IsInstalled()).Select(d => d.Value);
    }

    /// <summary>
    /// Checks the file system to determine if any dependencies are missing.
    /// </summary>
    /// <returns></returns>
    public bool IsMissingDependencies()
    {
        return GetMissingDependencies().Any();
    }

    /// <summary>
    /// Installs or updates the given NuGet dependency in the project.
    /// </summary>
    /// <param name="dependency"></param>
    /// <param name="outputDirectory"></param>
    /// <param name="version"></param>
    /// <param name="prerelease"></param>
    /// <param name="noCash"></param>
    /// <param name="requireConsent"></param>
    /// <param name="solutionDirectory"></param>
    /// <returns></returns>
    public InstallResult InstallDependency(string dependency, string outputDirectory = null, string version = null, bool? prerelease = null, bool? noCash = null, bool? requireConsent = null, string solutionDirectory = null)
    {
        if (Dependencies.ContainsKey(dependency))
        {
            UninstallDependency(dependency);
        }
        string output = NuGetCommand(string.Format("install {0}", dependency), new
        {
            o = outputDirectory,
            version = version
        });
        if (output != null)
        {
            var matches = findVersionRegex.Matches(output);
            if (matches.Count > 0)
            {
                var match = matches.Cast<Match>().Last();
                var dep = new NuGetDependency(match.Groups["version"].ToString(), dependency,
                    AbsolutePackagesDirectory);
                Dependencies.Add(dependency, dep);
                return dep.RemoveUnsupportedFrameworks(Frameworks)
                    ? InstallResult.InstallSucceeded
                    : InstallResult.InstallSucceededButNoSupportedFrameworks;
            }
            return InstallResult.InstallSucceeded;
        }
        else
        {
            return InstallResult.InstallFailed;
        }
    }

    public bool UninstallDependency(string dependency)
    {
        NuGetDependency dep;
        if (Dependencies.TryGetValue(dependency, out dep))
        {
            if (dep.Uninstall())
            {
                Dependencies.Remove(dependency);
                return true;
            }
        }
        return false;
    }

    private string NuGetCommand(string command, object options = null)
    {
        StringBuilder s = new StringBuilder(command);
        if (options != null)
        {
            s.AppendFormat(" {0}", string.Join(" ", options
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Select(p => new { name = p.Name, val = p.GetValue(options, null) })
                .Where(p => p.val != null && p.val.ToString() != "")
                .Select(p => string.Format("-{0} {1}", p.name, p.val)).ToArray()));
        }
        Debug.Log(string.Format("NuGetCommand: '{0}'", s));
        string workingDirectory = Application.dataPath;

        var process = Process.Start(new ProcessStartInfo(Util.CombinePaths(workingDirectory, "UnityNuGet", "Editor", "Lib", "NuGet", "nuget.exe"), s.ToString())
        {
            CreateNoWindow = true,
            WorkingDirectory = AbsolutePackagesDirectory,
            RedirectStandardOutput = true,
            UseShellExecute = false
        });

        if (process != null)
        {
            using (StreamReader stream = process.StandardOutput)
            {
                string output = stream.ReadToEnd();
                if (process.WaitForExit(20000) && process.ExitCode == 0)
                {
                    return output;
                }
            }
        }
        return null;
    }
}
