using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [TestFixture]
    public class DependenciesConfigTests
    {
        private static readonly Type[] s_ignoredTypes =
        {
            typeof(IBootstrapper),
        };

        private static readonly Type[] s_explicitTypes =
        {
            typeof(HttpRequestMessage)
        };

        private static readonly Type[] s_explicitGenericTypes =
        {
            typeof(IIdGenerator<Guid>),
            typeof(IValidator<TodoListItem>),
            typeof(IValidationCriterion<TodoListItem>),
            typeof(IResolvedItem<TodoListItem>),
            typeof(IItemWithErrors<TodoListItem>),
        };


        [Test]
        public void UnityContainer_AfterDependencyRegistration_ContainsAllContracts()
        {
            var exportedTypes = GetTypesExportedFromAssembly();
            var actualTypes = new List<Type>();
            var container = MockUnityContainer(actualTypes);

            DependenciesConfig.RegisterAllDependencies(container);
            var unexpectedTypes = actualTypes
                .Except(exportedTypes)
                .ToArray();
            var missingTypes = exportedTypes
                .Except(actualTypes)
                .ToArray();

            Assert.That(unexpectedTypes, Is.Empty, "There are more types registered to the container than expected.");
            Assert.That(missingTypes, Is.Empty, "Some of the types are not registered.");
        }


        private static Type[] GetTypesExportedFromAssembly()
            => typeof(IBootstrapper)
                .Assembly
                .ExportedTypes
                .Where(contract => contract.IsInterface)
                .Except(s_ignoredTypes)
                .Except(s_explicitGenericTypes.Select(type => type.GetGenericTypeDefinition()))
                .Union(s_explicitTypes)
                .Union(s_explicitGenericTypes)
                .ToArray();


        private static IUnityContainer MockUnityContainer(ICollection<Type> actualTypes)
        {
            var container = Substitute.For<IUnityContainer>();
            container
                .RegisterType(Arg.Any<Type>(), Arg.Any<Type>(), Arg.Any<string>(), Arg.Any<LifetimeManager>(), Arg.Any<InjectionMember[]>())
                .Returns(callInfo =>
                {
                    var typeFrom = callInfo.ArgAt<Type>(0);
                    var typeTo = callInfo.ArgAt<Type>(1);

                    actualTypes.Add(typeFrom ?? typeTo);

                    return container;
                });

            return container;
        }
    }
}