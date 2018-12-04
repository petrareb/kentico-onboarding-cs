using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ResolvedItemEqualityComparer : IEqualityComparer<IResolvedItem<TodoListItem>>
    {
        public bool Equals(IResolvedItem<TodoListItem> x, IResolvedItem<TodoListItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null
                || y is null
                || x.GetType() != y.GetType())
                return false;
            return new ItemEqualityComparer().Equals(x.Item, y.Item);
        }


        public int GetHashCode(IResolvedItem<TodoListItem> obj)
        {
            unchecked
            {
                return (EqualityComparer<TodoListItem>
                            .Default
                            .GetHashCode(obj.Item) * 397);
            }
        }
    }
}