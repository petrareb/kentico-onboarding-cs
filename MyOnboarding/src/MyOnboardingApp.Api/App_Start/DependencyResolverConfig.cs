using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Database.Repository;
using Unity;

namespace MyOnboardingApp.Api
{
    public static class DependencyResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            TodoListRepositoryConfig.Register(container, config);
            config.DependencyResolver = new DependencyResolver(container);
        }
    }
}