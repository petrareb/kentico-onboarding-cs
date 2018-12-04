using System;
using MyOnboardingApp.Contracts.Database;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Database.Repository;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Database
{
    public class DatabaseBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<ITodoListRepository, TodoListRepository>(new HierarchicalLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            if (!container.IsRegistered<IDatabaseConnection>())
            {
                throw new InvalidOperationException(
                    $"Configuration of {nameof(DatabaseBootstrapper)} is invalid, there must registered instance of {nameof(IDatabaseConnection)}."
                );
            }
        }
    }
}