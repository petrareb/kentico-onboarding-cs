using Unity;

namespace MyOnboardingApp.Contracts.Configuration
{
    public interface IConfiguration
    {
        void Register(IUnityContainer container);
    }
}