using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Mocks;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Api.Tests
{
    public class DependenciesConfigIntegrationTests
    {
        private static readonly Type[] s_ignoredTypes =
        {
            typeof(IBootstrapper),
            typeof(IItemWithErrors<>),
            typeof(IResolvedItem<>),
            typeof(IRegistrableBootstrap),
            typeof(IValidatedBootstrap)
        };

        private static readonly Type[] s_explicitTypes =
        {
            typeof(HttpRequestMessage),
            typeof(IIdGenerator<Guid>),
            typeof(IValidationCriterion<TodoListItem>),
        };


        [Test]
        public void UnityContainer_AfterDependencyRegistration_ContainsAllContracts()
        {
            var exportedTypes = GetTypesExportedFromAssembly();
            var (bootstrap, registeredTypes) = UnityContainerMock.CreateMock();

            bootstrap
                .RegisterAllDependencies();

            var unexpectedTypes = registeredTypes
                .Except(exportedTypes)
                .ToArray();
            var missingTypes = exportedTypes
                .Except(registeredTypes)
                .ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(unexpectedTypes, Is.Empty, "There are more types registered to the container than expected.");
                Assert.That(missingTypes, Is.Empty, "Some of the types are not registered.");
            });
        }


        [Test]
        public void Bootstrap_AfterDependencyRegistration_PassesValidation()
        {
            var registeredBootstrappers = new List<IBootstrapper>();

            var bootstrap = Bootstrap
                .Create(new UnityContainer(), registeredBootstrappers);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => bootstrap
                    .RegisterAllDependencies()
                    .ValidateConfiguration()
                );
                Assert.That(registeredBootstrappers, Is.Not.Empty);
            });
        }


        private static Type[] GetTypesExportedFromAssembly()
        {
            var explicitGenerics = s_explicitTypes
                .Where(type => type.IsGenericType)
                .ToArray();
            return typeof(IBootstrapper)
                .Assembly
                .ExportedTypes
                .Where(contract => contract.IsInterface)
                .Except(explicitGenerics.Select(type => type.GetGenericTypeDefinition()))
                .Except(s_ignoredTypes)
                .Union(s_explicitTypes)
                .ToArray();
        }
    }
}