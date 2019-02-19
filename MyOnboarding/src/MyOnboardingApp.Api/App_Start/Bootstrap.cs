using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Registration;
using Unity;

namespace MyOnboardingApp.Api
{
    internal class Bootstrap : IRegistrableBootstrap, IValidatedBootstrap
    {
        public IUnityContainer Container { get; }

        public ICollection<IBootstrapper> Bootstrappers { get; }


        public static Bootstrap Create(IUnityContainer container, ICollection<IBootstrapper> bootstrappers)
            => new Bootstrap(container, bootstrappers);


        private Bootstrap(IUnityContainer container, ICollection<IBootstrapper> bootstrappers)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            Bootstrappers = bootstrappers ?? throw new ArgumentNullException(nameof(bootstrappers));
        }


        public IRegistrableBootstrap RegisterDependency<TBootstrapper>()
            where TBootstrapper : IBootstrapper, new()
        {
            var bootstrapper = new TBootstrapper();

            bootstrapper.Register(Container);
            Bootstrappers.Add(bootstrapper);

            return this;
        }


        public IValidatedBootstrap ValidateConfiguration()
        {
            if (!Bootstrappers.Any())
                throw new InvalidOperationException(
                    "No Bootstrappers have been registered before checking the validity of their configuration.");

            var exceptions = new List<Exception>();
            foreach (var bootstrapper in Bootstrappers)
            {
                Validate(bootstrapper, exceptions);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }

            return this;
        }


        private void Validate(IBootstrapper bootstrapper, ICollection<Exception> exceptions)
        {
            try
            {
                bootstrapper.ValidateConfiguration(Container);
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
        }
    }
}