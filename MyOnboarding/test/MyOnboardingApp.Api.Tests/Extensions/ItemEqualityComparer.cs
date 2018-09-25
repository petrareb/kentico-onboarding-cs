using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Tests.Extensions
{
    public sealed class ItemEqualityComparer : IEqualityComparer<TodoListItem>
    {
        public bool Equals(TodoListItem x, TodoListItem y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id.Equals(y.Id) && string.Equals(x.Text, y.Text);
        }

        public int GetHashCode(TodoListItem obj)
        {
            unchecked
            {
                return (obj.Id.GetHashCode() * 397) ^ obj.Text.GetHashCode();
            }
        }
    }
}