using MyOnboardingApp.TestUtils.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.TestUtils.Extensions
{
    public static class ItemEqualityExtensions
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(ItemEqualityComparer.Instance);


        public static EqualConstraint UsingItemWithErrorsEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(ItemWithErrorsEqualityComparer.Instance);


        public static EqualConstraint UsingResolvedItemEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(ResolvedItemEqualityComparer.Instance);
    }
}