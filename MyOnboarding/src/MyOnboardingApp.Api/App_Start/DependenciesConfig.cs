using System.Runtime.CompilerServices;
using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Urls;
using MyOnboardingApp.Database;
using Unity;
using Unity.Lifetime;

[assembly: InternalsVisibleTo("MyOnboardingApp.Api.Tests")]

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
                .RegisterType<IRoutesConfig, RoutesConfig>(new HierarchicalLifetimeManager());


        private static IUnityContainer RegisterDependency<TDependency>(this IUnityContainer container) where TDependency : IBootstrapper, new()
        {
            var dependency = new TDependency();
            dependency.Register(container);
            return container;
        }
    }
}