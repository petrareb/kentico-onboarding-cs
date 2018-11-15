using MyOnboardingApp.TestUtils.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.TestUtils.Extensions
{
    internal static class ItemEqualityExtensions
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint) 
            => constraint.Using(new ItemEqualityComparer());
    }
}