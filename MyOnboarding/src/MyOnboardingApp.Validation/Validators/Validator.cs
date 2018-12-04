using System.Linq;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Validation.Validators
{
    internal class Validator<T> : IInvariantValidator<T>
        where T : class
    {
        private readonly IValidationCriterion<T>[] _criteria;


        public Validator(IValidationCriterion<T>[] criteria)
            => _criteria = criteria;


        public IItemWithErrors<T> Validate(T item)
        {
            var errors = _criteria
                .SelectMany(criterion => criterion.Validate(item))
                .ToList()
                .AsReadOnly();

            return ItemWithErrors.Create(item, errors);
        }
    }
}