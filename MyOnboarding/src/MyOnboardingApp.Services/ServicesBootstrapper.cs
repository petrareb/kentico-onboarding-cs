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
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Services
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container) => container
            .RegisterType<IIdGenerator<Guid>, GuidGenerator>(new HierarchicalLifetimeManager())
            .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager())
            .RegisterType<IValidationCriterion<TodoListItem>, TrimmedTextLengthCriterion>(nameof(TrimmedTextLengthCriterion), new SingletonLifetimeManager())
            .RegisterType<IValidationCriterion<TodoListItem>, TrimmedTextNonEmptyCriterion>(nameof(TrimmedTextNonEmptyCriterion), new SingletonLifetimeManager())
            .RegisterType<IValidationCriterion<TodoListItem>, LastUpdateDateBiggerThanCreationDateCriterion>(nameof(LastUpdateDateBiggerThanCreationDateCriterion), new SingletonLifetimeManager())
            .RegisterType<ICreateItemService, CreateItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IRetrieveItemService, RetrieveItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IDeleteItemService, DeleteItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IUpdateItemService, UpdateItemService>(new HierarchicalLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            if (container
                .Registrations
                .Where(registration => registration
                    .RegisteredType
                    .GetInterfaces()
                    .Where(registeredInterface => registeredInterface.IsGenericTypeDefinition)
                    .Select(registeredInterface => registeredInterface.GetGenericTypeDefinition())
                    .Any(registeredInterface => registeredInterface == typeof(IInvariantValidator<>)))
                .Skip(1)
                .Any())
                throw new InvalidOperationException($"Configuration of {nameof(ServicesBootstrapper)} is invalid, there must be only one instance of {nameof(IInvariantValidator<Type>)}.");

            if (!container.IsRegistered<ITodoListRepository>())
            {
                throw new InvalidOperationException($"Configuration of {nameof(ServicesBootstrapper)} is invalid, there must registered an instance of {nameof(ITodoListRepository)}.");
            }
        }
    }
}