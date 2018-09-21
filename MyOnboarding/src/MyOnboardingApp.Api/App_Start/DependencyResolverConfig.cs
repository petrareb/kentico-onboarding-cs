using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.Api.Utils;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.UrlLocation;
using MyOnboardingApp.Database;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Api
{
    public static class DependencyResolverConfig
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
}