using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Urls;
using MyOnboardingApp.Database;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Api
{
    public static class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterType<IUrlLocatorConfig, UrlLocatorConfig>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new DependencyResolver(container);
        }
    }

    public static class UnityContainerExtension
    {
        public static IUnityContainer RegisterDependency<TDependency>(this IUnityContainer container) where TDependency : IBootstrapper, new()
        {
            var dependency = new TDependency();
            dependency.Register(container);
            return container;
        }
    }
}


