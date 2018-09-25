using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Configuration;
using MyOnboardingApp.Contracts.UrlLocation;
using MyOnboardingApp.Database;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Api
{
    public static class DependenciesConfig /*DependencyResolverConfig*/
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterType<IUrlLocatorConfig, UrlLocatorConfig>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new DependencyResolver(container);
        }

        //public static IUnityContainer RegisterDependency<T>(this IUnityContainer container) where T : IConfiguration, new()
        //{
        //    var dependency = new T();
        //    dependency.Register(container);
        //    return container;
        //}
    }

    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterDependency<T>(this IUnityContainer container) where T : IConfiguration, new()
        {
            var dependency = new T();
            dependency.Register(container);
            return container;
        }
    }
}


