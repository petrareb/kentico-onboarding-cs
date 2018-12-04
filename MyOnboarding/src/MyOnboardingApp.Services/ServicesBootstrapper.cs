using System;
using System.Linq;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Criteria.DateCheckingCriteria;
using MyOnboardingApp.Services.Criteria.TextCheckingCriteria;
using MyOnboardingApp.Services.Generators;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.Services.Validation;
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
                .RegisterType<IValidator<TodoListItem>, TextCheckingValidator>(new HierarchicalLifetimeManager())
                .RegisterType<ICreateItemService, CreateItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IRetrieveItemService, RetrieveItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IDeleteItemService, DeleteItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IUpdateItemService, UpdateItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IItemWithErrors<TodoListItem>>(new HierarchicalLifetimeManager())
                .RegisterType<IResolvedItem<TodoListItem>>(new HierarchicalLifetimeManager())
                .RegisterType<IValidationCriterion<TodoListItem>>(new HierarchicalLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            var modelsWithMultipleValidators = GetModelsWithMultipleValidators(container);
            if (modelsWithMultipleValidators.Any())
            {
                throw new InvalidOperationException($"Only one implementation of {nameof(IInvariantValidator<Type>)} can be registered in {nameof(IUnityContainer)} " +
                                                    $"for each model, because TodoListItem services are only capable of accepting latest registered validator. " +
                                                    $"Following types have registered multiple validators: {string.Join(", ", modelsWithMultipleValidators)}");
            }

            if (!container.IsRegistered<ITodoListRepository>())
            {
                throw new InvalidOperationException($"An implementation of {nameof(ITodoListRepository)} is not registered in {nameof(IUnityContainer)}, " +
                                                    $"in order for TodoListItem services to access items in database.");
            }
        }


        private static string[] GetModelsWithMultipleValidators(IUnityContainer container)
        => container
            .Registrations
            .SelectMany(registration => registration
                .RegisteredType
                .GetInterfaces()
                .Where(registeredInterface => registeredInterface.IsGenericTypeDefinition)
                .Select(registeredInterface => registeredInterface.GetGenericTypeDefinition())
                .Where(registeredInterface => registeredInterface == typeof(IInvariantValidator<>)))
            .GroupBy(type => type.GenericTypeArguments
                .First())
            .Where(typeGroup => typeGroup
                .Skip(1)
                .Any())
            .Select(typeGroup => typeGroup.Key.FullName)
            .OrderBy(typeName => typeName)
            .ToArray();
    }
}