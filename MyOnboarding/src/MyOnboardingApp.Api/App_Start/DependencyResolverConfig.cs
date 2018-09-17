using System.Web.Http;
using MyOnboardingApp.Api.DependencyResolvers;
using MyOnboardingApp.Api.Utils;
using MyOnboardingApp.Contracts.Configuration;
using MyOnboardingApp.Database.Repository;
using Unity;

namespace MyOnboardingApp.Api
{
    public class DependencyResolverConfig: IConfiguration
    {
        // TODO UrlLocator dat na ApiServicesBootstraper a spravit internal ako DatabaseInternal, z tadeto zaregistrovat
        public void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterDependency<DatabaseBootstraper>()
                .RegisterDependency<ApiServicesBootstraper>();
            //navazuju na seba, preto treba vracat typ toho
            config.DependencyResolver = new DependencyResolver(container);
        }
    }
}