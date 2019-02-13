using System;
using System.Linq.Expressions;
using System.Reflection;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Validation.ErrorFactories
{
    public class ErrorFactory : IErrorFactory
    {
        public Error CreateValidationError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription)
        {
            var propertyInfo = GetPropertyInfoFromExpression(propertySelector);
            return new Error(ErrorCode.DataValidationError, errorDescription, propertyInfo.Name);
        }


        // just for illustration
        public Error CreatePermissionError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription)
        {
            var propertyInfo = GetPropertyInfoFromExpression(propertySelector);
            return new Error(ErrorCode.PermissionError, errorDescription, propertyInfo.Name);
        }


        private static PropertyInfo GetPropertyInfoFromExpression<TResult>(Expression<Func<TResult>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression 
                                   ?? throw new InvalidOperationException(
                                       "Error Factory supports only property selector, which body is a member expression in order to select a member property.");
            return memberExpression.Member as PropertyInfo 
                   ?? throw new InvalidOperationException(
                       "Error Factory supports only property selector with member expression in its body, that contains a property information.");
        }
    }
}