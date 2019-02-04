using System.Linq;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Validation.Validators
{
    internal class InvariantValidator<T> : IInvariantValidator<T>
        where T : class
    {
        private readonly IValidationCriterion<T>[] _criteria;


        public InvariantValidator(IValidationCriterion<T>[] criteria, IErrorFactory errorFactory)
        {
            _criteria = criteria;
            _errorFactory = errorFactory;
        }


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