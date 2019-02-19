using MyOnboardingApp.Api.Configuration;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Urls;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Api
{
    internal class ApiBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<IRoutesConfig, RoutesConfig>(new HierarchicalLifetimeManager())
                .RegisterType<IDatabaseConnection, DatabaseConnection>(new HierarchicalLifetimeManager());


        public void ValidateConfiguration(IUnityContainer container)
        {
            // nothing to worry about here ;-)
        }
    }
}
