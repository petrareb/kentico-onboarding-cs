using System;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Services.Generators;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Services
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public void Register(IUnityContainer container)
            => container
                .RegisterType<IIdGenerator<Guid>, GuidGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager());
    }
}