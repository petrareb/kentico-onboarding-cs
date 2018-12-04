using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.DateCheckingCriteria
{
    public class LastUpdateDateBiggerThanCreationDateCriterion : IValidationCriterion<TodoListItem>
    {
        private readonly IErrorFactory _errorFactory;


        public LastUpdateDateBiggerThanCreationDateCriterion(IErrorFactory errorFactory)
            => _errorFactory = errorFactory;


        public IEnumerable<Error> Validate(TodoListItem item)
        {
            if (item.CreationTime > item.LastUpdateTime)
            {
                yield return _errorFactory.CreateValidationError(() => item.CreationTime, "Creation Time must not be bigger than the time of last modification.");
            }
        }
    }
}