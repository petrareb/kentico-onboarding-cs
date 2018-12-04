using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ResolvedItemEqualityComparer : IEqualityComparer<IResolvedItem<TodoListItem>>
    {
        private static readonly Lazy<IEqualityComparer<IResolvedItem<TodoListItem>>> s_resolvedItemComparer
            = new Lazy<IEqualityComparer<IResolvedItem<TodoListItem>>>(() => new ResolvedItemEqualityComparer());

        public static IEqualityComparer<IResolvedItem<TodoListItem>> Instance
            => s_resolvedItemComparer.Value;


        private ResolvedItemEqualityComparer()
        {
        }


        public bool Equals(IResolvedItem<TodoListItem> x, IResolvedItem<TodoListItem> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (AreElementarilyDistinct(x, y))
            {
                return false;
            }

            return AreItemsInsideEqual(x, y) 
                   && AreOtherPropertiesEqual(x, y);
        }


        public int GetHashCode(IResolvedItem<TodoListItem> obj) 
            => obj.Item?.Id.GetHashCode() 
               ?? obj.WasOperationSuccessful.GetHashCode();


        private static bool AreElementarilyDistinct(IResolvedItem<TodoListItem> x, IResolvedItem<TodoListItem> y)
            => x is null
               || y is null
               || x.GetType() != y.GetType();


        private static bool AreOtherPropertiesEqual(IResolvedItem<TodoListItem> x, IResolvedItem<TodoListItem> y)
            => x.WasOperationSuccessful == y.WasOperationSuccessful;


        private static bool AreItemsInsideEqual(IResolvedItem<TodoListItem> x, IResolvedItem<TodoListItem> y)
            => ItemEqualityComparer
                .Instance
                .Equals(x.Item, y.Item);
    }
}