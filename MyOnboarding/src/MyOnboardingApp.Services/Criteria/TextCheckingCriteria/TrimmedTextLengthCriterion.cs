using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.TextCheckingCriteria
{
    public class TrimmedTextLengthCriterion : IValidationCriterion<TodoListItem>
    {
        internal const int MinLength = 2;
        internal const int MaxLength = 100;


        public IEnumerable<Error> Validate(TodoListItem item, IErrorFactory errorFactory)
        {
            var textLength = item.Text.Length;

            if (textLength < MinLength)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.Text,
                    $"Text of the item must be at least {MinLength} characters long.");
            }

            if (textLength > MaxLength)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.Text,
                    $"Text of the item must be at most {MaxLength} characters long.");
            }
        }
    }
}
