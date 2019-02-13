using System;
using System.Linq.Expressions;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Contracts.ErrorFactories
{
    public interface IErrorFactory
    {
        Error CreateValidationError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription);

        Error CreatePermissionError<TResult>(Expression<Func<TResult>> propertySelector, string errorDescription);
    }
}