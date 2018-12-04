using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.TextCheckingCriteria
{
    public class TrimmedTextLengthCriterion : IValidationCriterion<TodoListItem>
    {
        private readonly IErrorFactory _factory;
        internal const int MinLength = 2;
        internal const int MaxLength = 100;


        public TrimmedTextLengthCriterion(IErrorFactory factory)
            => _factory = factory;


        public IEnumerable<Error> Validate(TodoListItem item)
        {
            var textLength = item.Text.Length;
            const int minLength = 2;
            const int maxLength = 100;

            if (textLength < MinLength)
            {
                yield return _factory.CreateValidationError(
                    () => item.Text,
                    $"Text of the item must be at least {minLength} characters long.");
            }

            if (textLength > MaxLength)
            {
                yield return _factory.CreateValidationError(
                    () => item.Text,
                    $"Text of the item must be at most {maxLength} characters long.");
            }
        }
    }
}
