using NUnit.Framework.Constraints;

namespace MyOnboardingApp.Tests.Comparators
{
    public static class ItemEqualityExtension
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint) 
            => constraint.Using(new ItemEqualityComparer());
    }
}
