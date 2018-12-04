using MyOnboardingApp.Contracts.Validation;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Contracts.Registration
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterCriterion<TEntity, TCriterion>(this IUnityContainer container)
            where TEntity : class
            where TCriterion : IValidationCriterion<TEntity>
            => container.RegisterType<IValidationCriterion<TEntity>, TCriterion>(nameof(TCriterion), new SingletonLifetimeManager());
    }
}