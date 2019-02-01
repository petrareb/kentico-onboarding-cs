using System;

namespace MyOnboardingApp.Contracts.Validation
{
    public static class ResolvedItem
    {
        public static IResolvedItem<TEntity> Create<TEntity>(TEntity item)
            where TEntity : class
            => item == null
                ? ResolvedItemInternal<TEntity>.InvalidResolvedItem
                : new ResolvedItemInternal<TEntity>(item);


        private class ResolvedItemInternal<TEntity> : IResolvedItem<TEntity>
            where TEntity : class
        {
            public static readonly IResolvedItem<TEntity> InvalidResolvedItem = new ResolvedItemInternal<TEntity>();

            private readonly TEntity _item;

            public TEntity Item => _item ?? throw new InvalidOperationException($"Check the result of {nameof(WasOperationSuccessful)} first, please.");

            public bool WasOperationSuccessful => _item != null;


            public ResolvedItemInternal(TEntity item)
                : this()
                => _item = item ?? throw new ArgumentNullException(nameof(item));


            private ResolvedItemInternal()
            {
            }


            public override string ToString() 
                => $"{nameof(_item)}: {_item}, " +
                   $"{nameof(WasOperationSuccessful)}: {WasOperationSuccessful}";
        }
    }
}