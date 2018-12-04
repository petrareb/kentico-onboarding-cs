using MyOnboardingApp.TestUtils.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.TestUtils.Extensions
{
    internal static class ItemEqualityExtensions
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(new ItemEqualityComparer());


        public static EqualConstraint UsingItemWithErrorsEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(new ItemWithErrorsEqualityComparer());


        public static EqualConstraint UsingResolvedItemEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(new ResolvedItemEqualityComparer());
    }
}