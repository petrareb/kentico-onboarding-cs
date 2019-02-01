using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Errors;

namespace MyOnboardingApp.Contracts.Validation
{
    public static class ItemWithErrors
    {
        public static IItemWithErrors<TEntity> Create<TEntity>(TEntity item, IEnumerable<Error> errors)
            where TEntity : class
            => new ItemWithErrorsInternal<TEntity>(item, errors?.ToArray());


        private class ItemWithErrorsInternal<TEntity> : IItemWithErrors<TEntity>
            where TEntity : class
        {
            public TEntity Item { get; }

            public bool WasOperationSuccessful => Errors.Count == 0;

            public IReadOnlyCollection<Error> Errors { get; }


            public ItemWithErrorsInternal(TEntity item, IReadOnlyCollection<Error> errors)
            {
                Item = item
                       ?? throw new ArgumentNullException(
                           nameof(item),
                           $"Nonexistent item cannot have errors. Did you want to use {nameof(ResolvedItem)}?");

                Errors = errors ?? new List<Error>().AsReadOnly();
            }


            public override string ToString() 
                => $"{nameof(Item)}: {Item}, " +
                   $"{nameof(WasOperationSuccessful)}: {WasOperationSuccessful}, " +
                   $"{nameof(Errors)}: {Errors}";
        }
    }
}