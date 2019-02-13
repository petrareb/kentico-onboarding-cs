using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Factories;

namespace MyOnboardingApp.TestUtils.Extensions
{
    public static class ValidationCriterionExtensions
    {
        public static IEnumerable<string> GetErrorLocationsUsingFakeErrorFactory(this IValidationCriterion<TodoListItem> criterion, TodoListItem testItem)
        {
            var factory = new FakeErrorFactory();
            return criterion
                .Validate(testItem, factory)
                .Where(error => !string.IsNullOrEmpty(error.Message))
                .Select(error => error.Location);
        }
    }
}