using Unity;

namespace MyOnboardingApp.Contracts.Registration
{
    public interface IBootstrapper
    {
        IUnityContainer Register(IUnityContainer container);

        void ValidateConfiguration(IUnityContainer container);
    }
}