using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Criteria.DateCheckingCriteria
{
    public class LastUpdateDateBiggerThanCreationDateCriterion : IValidationCriterion<TodoListItem>
    {
        public IEnumerable<Error> Validate(TodoListItem item, IErrorFactory errorFactory)
        {
            if (item.CreationTime > item.LastUpdateTime)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.CreationTime, 
                    "Creation Time must not be bigger than the time of last modification.");
            }

            if (item.CreationTime == DateTime.MinValue)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.CreationTime,
                    $"Creation Time has a value of {DateTime.MinValue}, something went wrong...");
            }

            if (item.CreationTime == DateTime.MaxValue)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.CreationTime,
                    $"Creation Time has a value of {DateTime.MaxValue}, something went wrong...");
            }

            if (item.LastUpdateTime == DateTime.MinValue)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.LastUpdateTime,
                    $"Time of last update has a value of {DateTime.MinValue}, something went wrong...");
            }

            if (item.LastUpdateTime == DateTime.MaxValue)
            {
                yield return errorFactory.CreateValidationError(
                    () => item.LastUpdateTime,
                    $"Time of last update has a value of {DateTime.MaxValue}, something went wrong...");
            }
        }
    }
}