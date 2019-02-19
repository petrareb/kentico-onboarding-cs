using Unity;

namespace MyOnboardingApp.Contracts.Dependencies
{
    public interface IValidatedBootstrap
    {
        IUnityContainer Container { get; }
    }
}