using System.Net.Http;
using System.Web;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.Configuration;
using MyOnboardingApp.Contracts.UrlLocation;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MyOnboardingApp.ApiServices
{
    public class ApiServicesBootstrapper: IConfiguration
    {
        public void Register(IUnityContainer container)
        {
            container
                .RegisterType<HttpRequestMessage>(
                    new HierarchicalLifetimeManager(), 
                    new InjectionFactory(GetHttpRequestMessage))
                .RegisterType<IUrlLocator, ItemUrlLocator>(new HierarchicalLifetimeManager());
        }

        private static HttpRequestMessage GetHttpRequestMessage(IUnityContainer container) 
            => HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
    }
}
