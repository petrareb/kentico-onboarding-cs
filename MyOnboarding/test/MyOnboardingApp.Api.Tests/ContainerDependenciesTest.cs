using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyOnboardingApp.Api;
using MyOnboardingApp.Contracts.Registration;
using NUnit.Framework;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class ContainerDependenciesTest
    {
        private readonly IUnityContainer _container = new UnityContainerMock();
        private Type[] _exportedTypes;
        private readonly Type[] _ignoredTypes = 
        {
            typeof(IBootstrapper)
        };
        private readonly Type[] _explicitTypes =
        {
            typeof(HttpRequestMessage)
        };

        [Test]
        public void UnityContainer_AfterDependencyRegistration_ContainsAllContracts()
        {
            _exportedTypes = typeof(IBootstrapper)
                .Assembly
                .ExportedTypes
                .Where(contract => contract.IsInterface)
                .Except(_ignoredTypes)
                .Union(_explicitTypes)
                .ToArray();
            
            DependenciesConfig.RegisterAllDependencies(_container);
            var actualTypes = _container
                .Registrations
                .Select(registration => registration.RegisteredType)
                .ToArray();
            var unexpectedTypes = actualTypes
                .Except(_exportedTypes)
                .ToArray();
            var missingTypes = _exportedTypes
                .Except(actualTypes)
                .ToArray();

            Assert.That(unexpectedTypes, Is.Empty);
            Assert.That(missingTypes, Is.Empty);


        }

        private class UnityContainerMock: IUnityContainer
        {
            private readonly List<IContainerRegistration> _registrations = new List<IContainerRegistration>();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type typeFrom, Type typeTo, string name, LifetimeManager lifetimeManager,
                params InjectionMember[] injectionMembers)
            {
                _registrations.Add(new ContainerRegistration(typeFrom, name, typeTo, lifetimeManager));
                return this;
            }

            public IUnityContainer RegisterInstance(Type type, string name, object instance, LifetimeManager lifetime)
            {
                throw new NotImplementedException();
            }

            public object Resolve(Type type, string name, params ResolverOverride[] resolverOverrides)
            {
                throw new NotImplementedException();
            }

            public object BuildUp(Type type, object existing, string name, params ResolverOverride[] resolverOverrides)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer AddExtension(UnityContainerExtension extension)
            {
                throw new NotImplementedException();
            }

            public object Configure(Type configurationInterface)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer CreateChildContainer()
            {
                throw new NotImplementedException();
            }

            public bool IsRegistered(Type type, string name)
            {
                return Registrations
                    .Select(registration => registration.RegisteredType)
                    .Contains(type);
            }

            public IUnityContainer Parent { get; }
            public IEnumerable<IContainerRegistration> Registrations => _registrations;
        }   
    }
}