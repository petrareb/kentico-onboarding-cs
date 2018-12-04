using System;

namespace MyOnboardingApp.Contracts.Validation
{
    public static class ResolvedItem
    {
        public static IResolvedItem<TEntity> Create<TEntity>(TEntity item)
            where TEntity : class
            => item == null
                ? ResultItem<TEntity>.InvalidResolvedItem
                : new ResultItem<TEntity>(item);


        private class ResultItem<TEntity> : IResolvedItem<TEntity>
            where TEntity : class
        {
            public static readonly IResolvedItem<TEntity> InvalidResolvedItem = new ResultItem<TEntity>();

            private readonly TEntity _item;

            public TEntity Item => _item ?? throw new InvalidOperationException($"Check the result of {nameof(WasOperationSuccessful)} first, please.");

            public bool WasOperationSuccessful => _item != null;


            public ResultItem(TEntity item)
                : this()
                => _item = item ?? throw new ArgumentNullException(nameof(item));


            private ResultItem()
            {
            }
        }
    }
}