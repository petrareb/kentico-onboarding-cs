using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Validation;
using NSubstitute;
using NUnit.Framework;
using Unity;
using Unity.Lifetime;
using Unity.Registration;

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
            var (bootstrap, registeredTypes) = MockUnityContainer();

            bootstrap
                .RegisterAllDependencies();

            var unexpectedTypes = registeredTypes
                .Except(exportedTypes)
                .ToArray();
            var missingTypes = exportedTypes
                .Except(registeredTypes)
                .ToArray();

            Assert.That(unexpectedTypes, Is.Empty, "There are more types registered to the container than expected.");
            Assert.That(missingTypes, Is.Empty, "Some of the types are not registered.");
        }


        [Test]
        public void Bootstrap_AfterDependencyRegistration_PassesValidation()
        {
            var registeredBootstrappers = new List<IBootstrapper>();

            var bootstrap = Bootstrap
                .Create(new UnityContainer(), registeredBootstrappers);

            Assert.DoesNotThrow(() => bootstrap
                .RegisterAllDependencies()
                .ValidateConfiguration()
            );
            Assert.That(registeredBootstrappers, Is.Not.Empty);
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


        private static (Bootstrap bootstrap, ICollection<Type> registeredTypes) MockUnityContainer()
        {
            var registeredTypes = new List<Type>();

            var container = Substitute.For<IUnityContainer>();
            container
                .RegisterType(Arg.Any<Type>(), Arg.Any<Type>(), Arg.Any<string>(), Arg.Any<LifetimeManager>(), Arg.Any<InjectionMember[]>())
                .Returns(callInfo =>
                {
                    var typeFrom = callInfo.ArgAt<Type>(0);
                    var typeTo = callInfo.ArgAt<Type>(1);

                    registeredTypes.Add(typeFrom ?? typeTo);

                    return container;
                });

            var bootstrap = Bootstrap.Create(container, new List<IBootstrapper>());

            return (bootstrap, registeredTypes);
        }
    }
}