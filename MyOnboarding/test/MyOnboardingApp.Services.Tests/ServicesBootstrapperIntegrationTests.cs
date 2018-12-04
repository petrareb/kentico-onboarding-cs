using System;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Validation;
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Services.Tests
{
    [TestFixture]
    public class ServicesBootstrapperIntegrationTests
    {
        public class FakeModel
        {
        }

        private readonly IInvariantValidator<TodoListItem> _todoListItemValidator =
            Substitute.For<IInvariantValidator<TodoListItem>>();

        private readonly IInvariantValidator<FakeModel> _fakeModelValidator =
            Substitute.For<IInvariantValidator<FakeModel>>();

        private readonly ITodoListRepository _repository = 
            Substitute.For<ITodoListRepository>();


        [Test]
        public void ValidateConfiguration_ContainerWithMultipleValidatorsSpecified_ThrowsException()
        {
            var container = new UnityContainer()
                .RegisterInstance(_todoListItemValidator)
                .RegisterInstance(_fakeModelValidator)
                .RegisterInstance(_repository);

            var servicesBootstrapper = new ServicesBootstrapper();

            Assert.Throws<InvalidOperationException>(() =>
            {
                servicesBootstrapper.ValidateConfiguration(container);
            });
        }


        [Test]
        public void ValidateConfiguration_ContainerWithOneValidatorSpecified_PassesValidation()
        {
            var container = new UnityContainer()
                .RegisterInstance(_todoListItemValidator)
                .RegisterInstance(_repository);
          
            var servicesBootstrapper = new ServicesBootstrapper();
            servicesBootstrapper.Register(container);

            Assert.DoesNotThrow(() =>
            {
                servicesBootstrapper.ValidateConfiguration(container);
            });
        }


        [Test]
        public void ValidateConfiguration_RepositoryNotRegistered_ThrowsException()
        {
            var container = new UnityContainer()
                .RegisterInstance(_todoListItemValidator);

            var servicesBootstrapper = new ServicesBootstrapper();
            servicesBootstrapper.Register(container);

            Assert.Throws<InvalidOperationException>(() =>
            {
                servicesBootstrapper.ValidateConfiguration(container);
            });
        }
    }
}