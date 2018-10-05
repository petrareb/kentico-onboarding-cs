using MyOnboardingApp.Api.Tests.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.Api.Tests.Extensions
{
    internal static class ItemEqualityExtensions
    {
        public static EqualConstraint UsingItemEqualityComparer(this EqualConstraint constraint) 
            => constraint.Using(new ItemEqualityComparer());
    }
}