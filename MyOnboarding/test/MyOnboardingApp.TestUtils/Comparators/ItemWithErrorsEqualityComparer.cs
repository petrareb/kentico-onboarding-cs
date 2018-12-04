using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ItemWithErrorsEqualityComparer : IEqualityComparer<IItemWithErrors<TodoListItem>>
    {
        public bool Equals(IItemWithErrors<TodoListItem> x, IItemWithErrors<TodoListItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null
                || y is null
                || x.GetType() != y.GetType())
                return false;
            var xErrorList = x.Errors.ToList();
            var yErrorList = y.Errors.ToList();
            return new ItemEqualityComparer()
                       .Equals(x.Item, y.Item)
                   && !xErrorList.Except(yErrorList).Any()
                   && !yErrorList.Except(x.Errors.ToList()).Any();
        }


        public int GetHashCode(IItemWithErrors<TodoListItem> obj)
        {
            unchecked
            {
                return (EqualityComparer<TodoListItem>
                            .Default
                            .GetHashCode(obj.Item) * 397) ^ (obj.Errors != null ? obj.Errors.GetHashCode() : 0);
            }
        }
    }
}