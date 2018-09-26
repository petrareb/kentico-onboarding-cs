using MyOnboardingApp.Tests.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.Tests.Extensions
{
    public static class ItemEqualityExtensions
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint) 
            => constraint.Using(new ItemEqualityComparer());
    }
}