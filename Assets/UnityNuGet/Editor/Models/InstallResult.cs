using UnityEngine;
using System.Collections;

/// <summary>
/// Defines a list of values that represent the result of the NuGet install operation.
/// </summary>
public enum InstallResult
{
    /// <summary>
    /// Defines that the installation succeeded.
    /// </summary>
    InstallSucceeded,

    /// <summary>
    /// Defines that the installation succeeded, but did not contain any supported frameworks.
    /// </summary>
    InstallSucceededButNoSupportedFrameworks,

    /// <summary>
    /// Defines that the installation failed.
    /// </summary>
    InstallFailed
}
