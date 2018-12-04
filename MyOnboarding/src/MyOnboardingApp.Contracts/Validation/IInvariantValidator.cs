using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Validation
{
    public interface IInvariantValidator<TEntity>
        where TEntity : class
    {
        IItemWithErrors<TEntity> Validate(TEntity item);
    }
}