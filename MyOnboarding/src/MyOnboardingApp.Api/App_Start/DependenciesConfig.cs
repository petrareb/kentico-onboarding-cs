using System.Collections.Generic;
using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Database;
using MyOnboardingApp.Services;
using MyOnboardingApp.Validation;
using Unity;

namespace MyOnboardingApp.Api
{
    internal static class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
            => new UnityContainer()
                .InitializeTuple()
                .RegisterAllDependencies()
                .ValidateConfiguration()
                .ConfigureDependencyResolver(config);


        private static IRegistrableBootstrap InitializeTuple(this IUnityContainer container)
            => Bootstrap
                .Create(container, new List<IBootstrapper>());


        internal static IRegistrableBootstrap RegisterAllDependencies(this IRegistrableBootstrap bootstrap)
            => bootstrap
                .RegisterDependency<ApiBootstrapper>()
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterDependency<ServicesBootstrapper>()
                .RegisterDependency<ValidationBootstrapper>();


        private static void ConfigureDependencyResolver(this IValidatedBootstrap bootstrap, HttpConfiguration config)
            => config
                .DependencyResolver = new DependencyResolver(bootstrap.Container);
    }
}
