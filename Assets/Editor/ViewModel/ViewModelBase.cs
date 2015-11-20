using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using JetBrains.Annotations;

/// <summary>
/// Defines a base class that provides common functionality for view models.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected internal virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
