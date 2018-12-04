using System;
using System.Collections.Generic;
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
            .RegisterType<ICreateItemService, CreateItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IRetrieveItemService, RetrieveItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IDeleteItemService, DeleteItemService>(new HierarchicalLifetimeManager())
            .RegisterType<IUpdateItemService, UpdateItemService>(new HierarchicalLifetimeManager())
            .RegisterCriterion<TodoListItem, TrimmedTextLengthCriterion>()
            .RegisterCriterion<TodoListItem, TrimmedTextNonEmptyCriterion>()
            .RegisterCriterion<TodoListItem, LastUpdateDateBiggerThanCreationDateCriterion>();


        public void ValidateConfiguration(IUnityContainer container)
        {
            // Checking correctness of Validator and Criteria (for all validated models) is responsibility of ValidationBootstrapper
            // Responsibility of this ServiceBootstrapper is to check an existence of exactly one implementation of generic IInvariantValidator<>
            const int numberOfRequiredValidators = 1;

            var registeredValidators = GetRegisteredValidators(container);
            if (registeredValidators.Count() != numberOfRequiredValidators)
            {
                throw new InvalidOperationException(
                    $"Exactly {numberOfRequiredValidators} implementation of {nameof(IInvariantValidator<Type>)} has to be registered in {nameof(IUnityContainer)}.");
            }

            if (!container.IsRegistered<ITodoListRepository>())
            {
                throw new InvalidOperationException($"An implementation of {nameof(ITodoListRepository)} is not registered in {nameof(IUnityContainer)}, " +
                                                    "in order for TodoListItem services to access items in database.");
            }
        }


        private static IEnumerable<Type> GetRegisteredValidators(IUnityContainer container)
            => container
                .Registrations
                .Select(reg => reg.RegisteredType)
                .Where(type => type.IsGenericType 
                               && type.GetGenericTypeDefinition() == typeof(IInvariantValidator<>))
                .ToArray();
    }
}