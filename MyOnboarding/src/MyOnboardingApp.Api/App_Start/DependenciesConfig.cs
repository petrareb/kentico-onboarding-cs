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
using Unity;
using Unity.Lifetime;


namespace MyOnboardingApp.Api
{
    internal static class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            RegisterAllDependencies(container);
            config.DependencyResolver = new DependencyResolver(container);
        }


        internal static void RegisterAllDependencies(IUnityContainer container) 
            => container
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterDependency<ServicesBootstrapper>()
                .RegisterType<IRoutesConfig, RoutesConfig>(new HierarchicalLifetimeManager())
                .RegisterType<IDatabaseConnection, DatabaseConnection>(new HierarchicalLifetimeManager());


        private static IUnityContainer RegisterDependency<TDependency>(this IUnityContainer container) where TDependency : IBootstrapper, new()
        {
            var dependency = new TDependency();
            dependency.Register(container);
            return container;
        }
    }
}