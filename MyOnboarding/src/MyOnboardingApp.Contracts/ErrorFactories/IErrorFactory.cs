using System;
using System.Linq.Expressions;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Contracts.ErrorFactories
{
    public interface IErrorFactory
    {
        Error CreateValidationError(Expression<Func<object>> propertySelector, string errorDescription);

        Error CreatePermissionError(Expression<Func<object>> propertySelector, string errorDescription);
    }
}