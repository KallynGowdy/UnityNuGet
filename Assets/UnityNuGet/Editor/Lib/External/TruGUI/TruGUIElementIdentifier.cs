using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TruGUIElementIdentifier : Attribute
{
    string className;
    public string ClassName
    {
        get
        {
            return className;
        }
    }

    string comments;
    public string Comments
    {
        get
        {
            return comments;
        }
    }


    public TruGUIElementIdentifier(string className)
    {
        this.className = className;
        this.comments = string.Empty;
    }

    public override string ToString()
    {
        return String.Format("{0}: {1}", className, comments);
    }
}
