using Unity;

namespace MyOnboardingApp.Contracts.Registration
{
    public interface IBootstrapper
    {
        void Register(IUnityContainer container);
    }
}