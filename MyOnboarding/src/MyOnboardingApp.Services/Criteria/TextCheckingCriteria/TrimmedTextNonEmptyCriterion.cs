using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.TextCheckingCriteria
{
    public class TrimmedTextNonEmptyCriterion : IValidationCriterion<TodoListItem>
    {
        public IEnumerable<Error> Validate(TodoListItem item, IErrorFactory errorFactory)
        {
            if (item.Text == null)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be null.");
            }

            if (string.IsNullOrEmpty(item.Text))
            {
                yield return errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                yield return errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be made of just whitespace characters.");
            }
        }
    }
}