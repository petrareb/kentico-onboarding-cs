using System.Collections.Generic;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Contracts.Validation
{
    public interface IValidationCriterion<in TEntity>
        where TEntity : class
    {
        IEnumerable<Error> Validate(TEntity entity);
    }
}