using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ItemWithErrorsEqualityComparer : IEqualityComparer<IItemWithErrors<TodoListItem>>
    {
        private static readonly Lazy<IEqualityComparer<IItemWithErrors<TodoListItem>>> s_itemWithErrorsComparer
            = new Lazy<IEqualityComparer<IItemWithErrors<TodoListItem>>>(() => new ItemWithErrorsEqualityComparer());

        public static IEqualityComparer<IItemWithErrors<TodoListItem>> Instance 
            => s_itemWithErrorsComparer.Value;


        private ItemWithErrorsEqualityComparer()
        {
        }


        public bool Equals(IItemWithErrors<TodoListItem> x, IItemWithErrors<TodoListItem> y)
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


        public int GetHashCode(IItemWithErrors<TodoListItem> obj) 
            => obj.Item?.Id.GetHashCode() ?? obj.Errors.GetHashCode();


        private static bool AreElementarilyDistinct(IItemWithErrors<TodoListItem> x, IItemWithErrors<TodoListItem> y) 
            => x is null
               || y is null
               || x.GetType() != y.GetType();


        private static bool AreItemsInsideEqual(IItemWithErrors<TodoListItem> x, IItemWithErrors<TodoListItem> y) 
            => ItemEqualityComparer
                .Instance
                .Equals(x.Item, y.Item);


        private static bool AreOtherPropertiesEqual(IItemWithErrors<TodoListItem> x, IItemWithErrors<TodoListItem> y) 
            => !x.Errors.Except(y.Errors).Any()
               && !y.Errors.Except(x.Errors).Any()
               && y.WasOperationSuccessful == x.WasOperationSuccessful;
    }
}