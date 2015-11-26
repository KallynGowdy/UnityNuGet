using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using TruGUI;
using UniRx;

public static class Util
{
    public static string CombinePaths(params string[] paths)
    {
        return paths.Aggregate<string, string>(null, (current, path) =>
        {
            if (path == null)
            {
                return "";
            }
            return current != null ? Path.Combine(current, path) : path;
        });
    }

    public static string BuildAssetsDirectory(string relativePath)
    {
        return CombinePaths(Application.dataPath, relativePath);
    }

    public static IDisposable BindTo<T, TView>(this IObservable<T> observable, TView view, Action<TView, T> binding)
    {
        return observable.ObserveOnMainThread().Subscribe(value => binding(view, value));
    }

    public static IDisposable BindCommand<TViewModel>(this IActionable actionable, TViewModel viewModel, Action<TViewModel> command)
    {
        return
            Observable.FromEvent(h => actionable.OnActionTaken += h, h => actionable.OnActionTaken -= h)
                .ObserveOnMainThread()
                .Subscribe(u => command(viewModel));
    }

    //public static IDisposable BindCommand<TViewModel>(this ITruGUIElement element, TViewModel viewModel, Action<TViewModel> command)
    //{
    //    var o = Observable.FromEvent(h => element.OnActionTaken += h, h => element.OnActionTaken -= h).ObserveOnMainThread();
    //    return new CompositeDisposable(
    //        ((IActionable)element).BindCommand(viewModel, command)
    //        );
    //}
}
