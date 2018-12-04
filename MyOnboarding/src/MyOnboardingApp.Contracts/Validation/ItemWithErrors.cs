using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyOnboardingApp.Contracts.Errors;


namespace MyOnboardingApp.Contracts.Validation
{
    public class ItemWithErrors
    {
        public static IItemWithErrors<TEntity> Create<TEntity>(TEntity item, IReadOnlyCollection<Error> errors)
            where TEntity : class
            => new ValidatedItem<TEntity>(item, errors);


        private class ValidatedItem<TEntity> : IItemWithErrors<TEntity>
            where TEntity : class
        {
            public TEntity Item { get; }

            public bool WasOperationSuccessful => Errors.Count == 0;

            public IReadOnlyCollection<Error> Errors { get; }


            public ValidatedItem(TEntity item, IReadOnlyCollection<Error> errors)
            {
                Item = item
                       ?? throw new ArgumentNullException(
                           nameof(item),
                           $"Nonexistent item cannot have errors. Did you want to use {nameof(ResolvedItem)}?");

                Errors = errors ?? new List<Error>().AsReadOnly();
            }
        }
    }
}