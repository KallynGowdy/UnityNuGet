using UnityEngine;
using System.Collections;

/// <summary>
/// Defines an interface for preferences.
/// </summary>
public interface IPreferences
{
    int GetInt(string key, int defaultValue = 0);
    void SetInt(string key, int value);

    string GetString(string key, string defaultValue = null);
    void SetString(string key, string value);

    bool GetBool(string key, bool defaultValue = false);
    void SetBool(string key, bool value);

    float GetFloat(string key, float defaultValue = 0f);
    void SetFloat(string key, float value);

    void DeleteKey(string key);
}
