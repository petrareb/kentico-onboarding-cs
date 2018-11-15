using System;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Generators;
using MyOnboardingApp.Services.Services;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Services
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public void Register(IUnityContainer container)
            => container
                .RegisterType<IIdGenerator<Guid>, GuidGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IItemWithErrors<TodoListItem>>(new HierarchicalLifetimeManager())
                .RegisterType<IResolvedItem<TodoListItem>>(new HierarchicalLifetimeManager())
                .RegisterType<IValidationCriterion<TodoListItem>>(new HierarchicalLifetimeManager())
                .RegisterType<IRetrieveItemService, RetrieveItemService>(new HierarchicalLifetimeManager());
    }
}