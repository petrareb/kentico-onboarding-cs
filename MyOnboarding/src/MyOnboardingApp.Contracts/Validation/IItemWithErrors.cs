using System.Collections.Generic;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Contracts.Validation
{
    public interface IItemWithErrors<out TItem> : IResolvedItem<TItem>
        where TItem : class
    {
        IReadOnlyCollection<Error> Errors { get; }
    }
}