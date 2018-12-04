using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyOnboardingApp.Api.Configuration;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Urls;
using MyOnboardingApp.Database;
using MyOnboardingApp.Services;
using MyOnboardingApp.Validation;
using Unity;
using Unity.Lifetime;


namespace MyOnboardingApp.Api
{
    internal static class DependenciesConfig
    {
        private static readonly IList<IBootstrapper> s_bootstrappers = new List<IBootstrapper>();


        public static void Register(HttpConfiguration config)
            => new UnityContainer()
                .RegisterAllDependencies()
                .ValidateConfiguration()
                .ConfigureDependencyResolver(config);


        internal static IUnityContainer RegisterAllDependencies(this IUnityContainer container)
            => container
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterDependency<ServicesBootstrapper>()
                .RegisterDependency<ValidationBootstrapper>()
                .RegisterType<IRoutesConfig, RoutesConfig>(new HierarchicalLifetimeManager())
                .RegisterType<IDatabaseConnection, DatabaseConnection>(new HierarchicalLifetimeManager());


        private static IUnityContainer RegisterDependency<TDependency>(this IUnityContainer container)
            where TDependency : IBootstrapper, new()
            => new TDependency()
                .StoreInstance()
                .Register(container);


        private static TDependency StoreInstance<TDependency>(this TDependency bootstrapper)
            where TDependency : IBootstrapper, new()
        {
            s_bootstrappers.Add(bootstrapper);

            return bootstrapper;
        }


        private static IUnityContainer ValidateConfiguration(this IUnityContainer container)
        {
            if (!s_bootstrappers.Any())
                throw new Exception("Bootstrappers have to be registered before checking validity of their configuration.");

            var exceptions = new List<Exception>();
            foreach (var bootstrapper in s_bootstrappers)
            {
                container.Validate(bootstrapper, exceptions);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }

            return container;
        }


        private static void Validate(this IUnityContainer container, IBootstrapper bootstrapper, ICollection<Exception> exceptions)
        {
            try
            {
                bootstrapper.ValidateConfiguration(container);
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
        }


        private static void ConfigureDependencyResolver(this IUnityContainer container, HttpConfiguration config)
            => config
                .DependencyResolver = new DependencyResolver(container);
    }
}