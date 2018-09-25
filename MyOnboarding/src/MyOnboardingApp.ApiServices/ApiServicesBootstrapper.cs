using System.Net.Http;
using System.Web;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.UrlLocation;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MyOnboardingApp.ApiServices
{
    public class ApiServicesBootstrapper: IRegistration
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
