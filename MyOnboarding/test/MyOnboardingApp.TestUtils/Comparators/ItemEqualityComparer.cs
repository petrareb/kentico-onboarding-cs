using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ItemEqualityComparer : IEqualityComparer<TodoListItem>
    {
        public bool Equals(TodoListItem x, TodoListItem y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null
                || y is null
                || x.GetType() != y.GetType())
                return false;
            return x.Id.Equals(y.Id)
                   && string.Equals(x.Text, y.Text)
                   && x.CreationTime.Equals(y.CreationTime)
                   && x.LastUpdateTime.Equals(y.LastUpdateTime);
        }


        public int GetHashCode(TodoListItem obj)
        {
            unchecked
            {
                var hashCode = obj.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Text.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.CreationTime.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.LastUpdateTime.GetHashCode();
                return hashCode;
            }
        }
    }
}