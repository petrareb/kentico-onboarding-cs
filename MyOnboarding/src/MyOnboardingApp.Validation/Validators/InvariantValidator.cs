using System;
using System.Linq;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Validation.Validators
{
    internal class InvariantValidator<T> : IInvariantValidator<T>
        where T : class
    {
        private readonly IValidationCriterion<T>[] _criteria;
        private readonly IErrorFactory _errorFactory;


        public InvariantValidator(IValidationCriterion<T>[] criteria, IErrorFactory errorFactory)
        {
            _criteria = criteria 
                        ?? throw new InvalidCastException(
                            "There are no criteria specified to define the functionality of the validator.");
            _errorFactory = errorFactory;
        }


        public IItemWithErrors<T> Validate(T item)
        {
            var errors = _criteria
                .SelectMany(criterion => criterion.Validate(item, _errorFactory))
                .ToArray();

            return ItemWithErrors.Create(item, errors);
        }
    }
}