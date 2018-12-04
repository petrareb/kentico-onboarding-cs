using System;
using System.Net.Http;
using System.Web;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Urls;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MyOnboardingApp.ApiServices
{
    public class ApiServicesBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<HttpRequestMessage>(
                    new HierarchicalLifetimeManager(),
                    new InjectionFactory(GetHttpRequestMessage))
                .RegisterType<IUrlLocator, ItemUrlLocator>(new HierarchicalLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            if (!container.IsRegistered<IRoutesConfig>())
            {
                throw new InvalidOperationException($"Configuration of {nameof(ApiServicesBootstrapper)} is invalid, there must registered instance of {nameof(IRoutesConfig)}.");
            }
        }


        private static HttpRequestMessage GetHttpRequestMessage(IUnityContainer container)
            => (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}
