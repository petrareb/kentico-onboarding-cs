using System;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Validation.ErrorFactories;
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
                typeof(Validator<>),
                new SingletonLifetimeManager())
            .RegisterType<IErrorFactory, ErrorFactory>(new SingletonLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            if (!container.IsRegistered<IErrorFactory>())
            {
                throw new InvalidOperationException(
                    $"Configuration of {nameof(ValidationBootstrapper)} is invalid, there must registered an array of {nameof(IErrorFactory)}.");
            }
        }
    }
}