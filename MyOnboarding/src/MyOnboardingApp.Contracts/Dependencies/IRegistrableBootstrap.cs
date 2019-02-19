using MyOnboardingApp.Contracts.Registration;

namespace MyOnboardingApp.Contracts.Dependencies
{
    public interface IRegistrableBootstrap
    {
        IRegistrableBootstrap RegisterDependency<TBootstrapper>()
            where TBootstrapper : IBootstrapper, new();


        IValidatedBootstrap ValidateConfiguration();
    }
}