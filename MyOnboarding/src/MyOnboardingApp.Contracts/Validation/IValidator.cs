using System.Collections.Generic;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Validation
{
    public interface IValidationCriterion<in TEntity>
        where TEntity : class
    {
        IEnumerable<string> Validate(TEntity entity);
    }

    public interface IValidator<TEntity>
        where TEntity : class
    {
        IItemWithErrors<TEntity> Validate(TEntity item);
    }
}