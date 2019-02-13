using System;
using System.Linq.Expressions;
using System.Reflection;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.TestUtils.Factories
{
    public class FakeErrorFactory: IErrorFactory
    {
        public Error CreateValidationError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription)
        {
            var memberExpression = (MemberExpression)propertySelector.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;

            return new Error(ErrorCode.DataValidationError, errorDescription, propertyInfo.Name);
        }


        public Error CreatePermissionError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription)
            => throw new NotImplementedException();
    }
}