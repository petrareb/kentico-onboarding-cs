using System.Web.Http;

namespace MyOnboardingApp.Contracts.Configuration
{
    public interface IConfiguration
    {
        void Register(HttpConfiguration config);
    }
}