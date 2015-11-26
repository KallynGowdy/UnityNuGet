using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
/// Defines a class that represents the result of a NuGet operation.
/// </summary>
public class NuGetOperationResult<TResult>
{
    /// <summary>
    /// Gets whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; private set; }
    
    /// <summary>
    /// Gets the array of errors that occurred during the operation.
    /// </summary>
    public string[] Errors { get; private set; }

    public TResult Result { get; private set; }

    public static NuGetOperationResult<TResult> Success(TResult result)
    {
        return new NuGetOperationResult<TResult>(true, null, result);
    }

    public static NuGetOperationResult<TResult> Error([NotNull] params string[] errors)
    {
        if (errors == null) throw new ArgumentNullException("errors");
        return new NuGetOperationResult<TResult>(false, errors.Where(e => e != null).ToArray(), default(TResult));
    }

    private NuGetOperationResult(bool success, string[] errors, TResult result)
    {
        Succeeded = success;
        Errors = errors;
        Result = result;
    }
}
