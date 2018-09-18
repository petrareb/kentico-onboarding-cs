using System.Web.Http;
using Unity;
using MyOnboardingApp.Api.Utils;
using UnityContainerExtensions = MyOnboardingApp.Api.Utils.UnityContainerExtensions;

namespace MyOnboardingApp.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //var container = new UnityContainer();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(JsonSerializerConfig.Register);
            GlobalConfiguration.Configure(DependencyResolverConfig.Register);
            //container.RegisterDependency<DependencyResolverConfig>();

        }
    }
}
