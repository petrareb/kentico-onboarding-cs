using System;
using System.Collections.Generic;

namespace MyOnboardingApp.Contracts.Models
{
    public class ItemWithErrors
    {
        public static IItemWithErrors<TEntity> Create<TEntity>(TEntity item, IReadOnlyCollection<string> errors)
            where TEntity : class
            => new ValidatedItem<TEntity>(item, errors);


        private class ValidatedItem<TEntity> : IItemWithErrors<TEntity>
            where TEntity : class
        {
            public TEntity Item { get; }

            public bool WasOperationSuccessful => Errors.Count == 0;

            public IReadOnlyCollection<string> Errors { get; }


            public ValidatedItem(TEntity item, IReadOnlyCollection<string> errors)
            {
                Item = item ?? throw new ArgumentNullException(nameof(item),
                           $"Nonexistent item cannot have errors. Did you want to use {nameof(ResolvedItem)}?");

                Errors = errors ?? new List<string>().AsReadOnly();
            }
        }
    }

    public interface IItemWithErrors<out TItem> : IResolvedItem<TItem>
        where TItem : class
    {
        IReadOnlyCollection<string> Errors { get; }
    }
}