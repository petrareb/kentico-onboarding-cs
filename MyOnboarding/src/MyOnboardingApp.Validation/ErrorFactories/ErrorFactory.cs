using System;
using System.Linq.Expressions;
using System.Reflection;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Validation.ErrorFactories
{
    public class ErrorFactory : IErrorFactory
    {
        public Error CreateValidationError(Expression<Func<object>> propertySelector, string errorDescription)
        {
            var propertyInfo = GetPropertyInfoFromExpression(propertySelector);
            return new Error(ErrorCode.DataValidationError, errorDescription, propertyInfo.Name);
        }


        // just for illustration
        public Error CreatePermissionError(Expression<Func<object>> propertySelector, string errorDescription)
        {
            var propertyInfo = GetPropertyInfoFromExpression(propertySelector);
            return new Error(ErrorCode.PermissionError, errorDescription, propertyInfo.Name);
        }


        private static PropertyInfo GetPropertyInfoFromExpression(Expression<Func<object>> propertySelector)
        {
            var memberExpression = (MemberExpression)propertySelector.Body;
            return (PropertyInfo)memberExpression.Member;
        }
    }
}