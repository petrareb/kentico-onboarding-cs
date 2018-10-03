using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyOnboardingApp.Contracts.Registration;
using NSubstitute;
using NUnit.Framework;
using Unity;
using Unity.Lifetime;
using Unity.Registration;

namespace MyOnboardingApp.Api.Tests
{
    [TestFixture]
    public class ContainerDependenciesTest
    {
        private Type[] _exportedTypes;
        private readonly Type[] _ignoredTypes = 
        {
            typeof(IBootstrapper)
        };
        private readonly Type[] _explicitTypes =
        {
            typeof(HttpRequestMessage)
        };

        public Type[] GetExportedTypesFromAssembly() 
            => typeof(IBootstrapper)
                .Assembly
                .ExportedTypes
                .Where(contract => contract.IsInterface)
                .Except(_ignoredTypes)
                .Union(_explicitTypes)
                .ToArray();

        public Type[] GetExtraTypesFromArray(Type[] array, Type[] expectedArray) 
            => array.Except(expectedArray).ToArray();

        public IUnityContainer MockUnityContainer(ICollection<Type> actualTypes)
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


        [Test]
        public void UnityContainer_AfterDependencyRegistration_ContainsAllContracts()
        {
            _exportedTypes = GetExportedTypesFromAssembly();
            var actualTypes = new List<Type>();
            var container = MockUnityContainer(actualTypes);
      
            DependenciesConfig.RegisterAllDependencies(container);
            var unexpectedTypes = GetExtraTypesFromArray(actualTypes.ToArray(), _exportedTypes);
            var missingTypes = GetExtraTypesFromArray(_exportedTypes, actualTypes.ToArray());
              
            Assert.That(unexpectedTypes, Is.Empty, "There are more types registered to the container than expected.");
            Assert.That(missingTypes, Is.Empty, "Some of the types are not registered.");
        } 
    }
}