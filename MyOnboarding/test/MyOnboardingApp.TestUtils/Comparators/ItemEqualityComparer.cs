using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ItemEqualityComparer : IEqualityComparer<TodoListItem>
    {
        private static readonly Lazy<ItemEqualityComparer> s_itemEqualityComparer 
            = new Lazy<ItemEqualityComparer>(() => new ItemEqualityComparer());

        public static IEqualityComparer<TodoListItem> Instance 
            => s_itemEqualityComparer.Value;


        private ItemEqualityComparer()
        {
        }


        public bool Equals(TodoListItem x, TodoListItem y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return !AreElementarilyDistinct(x, y) 
                   && ArePropertiesEqual(x, y);
        }


        public int GetHashCode(TodoListItem obj) 
            => obj.Id.GetHashCode();


        private static bool AreElementarilyDistinct(TodoListItem x, TodoListItem y) 
            => x is null
               || y is null
               || x.GetType() != y.GetType();


        private static bool ArePropertiesEqual(TodoListItem x, TodoListItem y) 
            => x.Id.Equals(y.Id)
               && string.Equals(x.Text, y.Text)
               && x.CreationTime.Equals(y.CreationTime)
               && x.LastUpdateTime.Equals(y.LastUpdateTime);
    }
}