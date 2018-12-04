using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.TextCheckingCriteria
{
    public class TrimmedTextNonEmptyCriterion : IValidationCriterion<TodoListItem>
    {
        private readonly IErrorFactory _errorFactory;


        public TrimmedTextNonEmptyCriterion(IErrorFactory factory)
            => _errorFactory = factory;


        public IEnumerable<Error> Validate(TodoListItem item)
        {
            if (item.Text == null)
            {
                yield return _errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be null.");
            }

            if (string.IsNullOrEmpty(item.Text))
            {
                yield return _errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                yield return _errorFactory.CreateValidationError(
                    () => item.Text,
                    "Text of the item must not be whitespace.");
            }
        }
    }
}