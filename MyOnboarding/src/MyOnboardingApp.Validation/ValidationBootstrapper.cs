using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Validation.ErrorFactories;
using MyOnboardingApp.Validation.Extensions;
using MyOnboardingApp.Validation.Validators;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Validation
{
    public class ValidationBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container) => container
            .RegisterType(
                typeof(IInvariantValidator<>),
                typeof(InvariantValidator<>),
                new SingletonLifetimeManager())
            .RegisterType<IErrorFactory, ErrorFactory>(new SingletonLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            // Check the equality of models registered for criteria and validator
            var validatedModels = GetModelsFromRegisteredValidators(container);
            var criteriaModels = GetModelsFromRegisteredCriteria(container);

            var extraValidatorModels = validatedModels
                .Except(criteriaModels)
                .ToArray();
            var extraCriteriaModels = criteriaModels
                .Except(validatedModels)
                .ToArray();

            if (extraValidatorModels.Any())
            {
                var extraModelNames = GetTypeNames(extraValidatorModels);
                throw new InvalidOperationException(
                    $"In {nameof(IUnityContainer)} there are injected validators for models that have no {typeof(IValidationCriterion<>)} registered. " +
                    $"The following models are validated, but miss criteria: {string.Join(", ", extraModelNames)}");
            }

            if (extraCriteriaModels.Any())
            {
                var extraModelNames = GetTypeNames(extraCriteriaModels);
                throw new InvalidOperationException(
                    $"In {nameof(IUnityContainer)} there are criteria registered for models that are not validated by an injected {typeof(IInvariantValidator<>)}." +
                    $"The following models are not validated: {string.Join(", ", extraModelNames)}");
            }

            if (!container.IsRegistered<IErrorFactory>())
            {
                throw new InvalidOperationException(
                    $"In {nameof(IUnityContainer)} there has to be registered an array of {nameof(IErrorFactory)}, " +
                    $"in order for the {typeof(IInvariantValidator<>)} to create various validation errors.");
            }
        }


        private static ICollection<Type> GetModelsFromRegisteredValidators(IUnityContainer container)
            => container
                .Registrations
                .GetConstructorsOfMappedTypes()
                .GetParametersFromConstructors()
                .GetParameterTypes()
                .SelectGenericTypesImplementingType(typeof(IInvariantValidator<>))
                .SelectArgumentsFromGenericTypes()
                .ToArray();


        private static ICollection<Type> GetModelsFromRegisteredCriteria(IUnityContainer container)
            => container
                .Registrations
                .GetRegisteredTypes()
                .SelectGenericTypesImplementingType(typeof(IValidationCriterion<>))
                .SelectArgumentsFromGenericTypes()
                .ToArray();


        private static string[] GetTypeNames(IEnumerable<Type> types) 
            => types
                .Select(type => type.FullName)
                .ToArray();
    }
}