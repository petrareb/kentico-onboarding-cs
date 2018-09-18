using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.Utils;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Database.Repository;
using Unity;

namespace MyOnboardingApp.Api
{
    public static class DependencyResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>();

            config.DependencyResolver = new DependencyResolver(container);
        }
    }
}