using System.Web.Http;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.UrlLocation;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.ApiServices
{
    public static class ApiServicesBootstrapper
    {
        public static void Register(UnityContainer container, HttpConfiguration config)
        {
            container.RegisterType<IUrlLocator, ItemUrlLocator>(new HierarchicalLifetimeManager());
        }
    }
}