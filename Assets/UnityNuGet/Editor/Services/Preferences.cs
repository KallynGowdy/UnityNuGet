using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Defines a class that is used to store preferences using Unity <see cref="EditorPrefs"/>.
/// </summary>
public class Preferences : IPreferences
{
    public int GetInt(string key, int defaultValue = 0)
    {
        return EditorPrefs.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        EditorPrefs.SetInt(key, value);
    }

    public string GetString(string key, string defaultValue = null)
    {
        return EditorPrefs.GetString(key, defaultValue);
    }

    public void SetString(string key, string value)
    {
        EditorPrefs.SetString(key, value);
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        return EditorPrefs.GetBool(key, defaultValue);
    }

    public void SetBool(string key, bool value)
    {
        EditorPrefs.SetBool(key, value);
    }

    public float GetFloat(string key, float defaultValue = 0)
    {
        return EditorPrefs.GetFloat(key, defaultValue);
    }

    public void SetFloat(string key, float value)
    {
        EditorPrefs.SetFloat(key, value);
    }

    public void DeleteKey(string key)
    {
        EditorPrefs.DeleteKey(key);
    }
}
