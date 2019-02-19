using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.TestUtils.Comparators
{
    internal sealed class ErrorComparer : IEqualityComparer<Error>
    {
        private static readonly Lazy<ErrorComparer> s_errorComparer 
            = new Lazy<ErrorComparer>(() => new ErrorComparer());

        public static IEqualityComparer<Error> Instance 
            => s_errorComparer.Value;


        private ErrorComparer()
        {
        }


        public bool Equals(Error x, Error y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return !AreElementarilyDistinct(x, y) 
                   && ArePropertiesEqual(x, y);
        }


        public int GetHashCode(Error obj)
        {
            unchecked
            {
                var hashCode = (obj.Message != null ? obj.Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)obj.Code;
                hashCode = (hashCode * 397) ^ (obj.Location != null ? obj.Location.GetHashCode() : 0);
                return hashCode;
            }
        }


        private static bool ArePropertiesEqual(Error x, Error y)
            => string.Equals(x.Message, y.Message)
               && x.Code == y.Code
               && string.Equals(x.Location, y.Location);


        private static bool AreElementarilyDistinct(Error x, Error y) 
            => x is null
               || y is null
               || x.GetType() != y.GetType();
    }
}