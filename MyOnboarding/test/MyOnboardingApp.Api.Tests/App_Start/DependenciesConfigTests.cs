using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Registration;
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Api.Tests
{
    [TestFixture]
    public class DependenciesConfigTests
    {
        [Test]
        public void ValidateConfiguration_NoBootstrappersInBootstrap_ThrowsException()
        {
            var bootstrap = MockBootstrap(Array.Empty<IBootstrapper>());

            Assert.Throws<InvalidOperationException>(
                () => bootstrap.ValidateConfiguration());
        }


        [Test]
        public void ValidateConfiguration_AllBootstrappersValidInUnityContainer_ReturnsContainer()
        {
            var bootstrapper = Substitute.For<IBootstrapper>();
            var bootstrappers = new [] { bootstrapper };
            var bootstrap = MockBootstrap(bootstrappers);

            var result = bootstrap.ValidateConfiguration();

            bootstrapper.Received(1).ValidateConfiguration(bootstrap.Container);
            Assert.That(result, Is.EqualTo(bootstrap));
        }


        [Test]
        public void ValidateConfiguration_SomeBootstrappersThrowException_ThrowsAggregateException()
        {
            var bootstrapper1 = Substitute.For<IBootstrapper>();
            var bootstrapper2 = Substitute.For<IBootstrapper>();
            var bootstrappers = new[] {bootstrapper1, bootstrapper2};
            var bootstrap = MockBootstrap(bootstrappers);
            bootstrapper1
                .When(b => b.ValidateConfiguration(bootstrap.Container))
                .Do(b => throw new Exception());

            Assert.Throws<AggregateException>(
                () => bootstrap.ValidateConfiguration());
        }


        private static Bootstrap MockBootstrap(ICollection<IBootstrapper> bootstrappers)
        {
            var container = Substitute.For<IUnityContainer>();
            var bootstrap = Bootstrap.Create(container, bootstrappers);

            return bootstrap;
        }
    }
}