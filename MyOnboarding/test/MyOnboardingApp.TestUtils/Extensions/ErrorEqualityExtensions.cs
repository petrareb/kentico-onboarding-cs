using MyOnboardingApp.TestUtils.Comparators;
using NUnit.Framework.Constraints;

namespace MyOnboardingApp.TestUtils.Extensions
{
    public static class ErrorEqualityExtensions
    {
        public static EqualConstraint UsingErrorEqualityComparer(this EqualConstraint constraint)
            => constraint.Using(ErrorComparer.Instance);
    }
}