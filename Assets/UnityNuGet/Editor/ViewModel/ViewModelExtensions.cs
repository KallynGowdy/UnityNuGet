using System;
using UnityEngine;
using System.Collections;
using System.Linq.Expressions;

/// <summary>
/// Extension methods for <see cref="ViewModelBase"/> objects.
/// </summary>
public static class ViewModelExtensions
{
    internal static void OnPropertyChanged<TViewModel, TProperty>(this TViewModel viewModel,
        Expression<Func<TViewModel, TProperty>> expr)
        where TViewModel : ViewModelBase
    {
        var member = expr.Body as MemberExpression;
        if (member != null)
        {
            viewModel.OnPropertyChanged(member.Member.Name);
        }
        else
        {
            throw new ArgumentException("The given expression must point to a specific member. (vm => vm.Member)", "expr");
        }
    }
}
