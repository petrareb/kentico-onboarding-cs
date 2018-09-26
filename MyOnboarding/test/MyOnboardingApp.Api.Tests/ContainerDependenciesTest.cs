using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using MyOnboardingApp.Api;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Urls;
using MyOnboardingApp.Database;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class ContainerDependenciesTest
    {
        private IUnityContainer _container;
        private List<Type> _exportedTypes;
        private List<Type> _ignoredTypes;

        [SetUp]
        public void SetUp()
        {
            _ignoredTypes = new List<Type>
            {
                typeof(DatabaseBootstrapper),
                typeof(ApiServicesBootstrapper),
                typeof(IBootstrapper)
            };
            _exportedTypes = Assembly
                .Load("MyOnboardingApp.Contracts")
                .ExportedTypes
                .Where(contract => contract.IsInterface)
                .ToList();
            _exportedTypes.Add(typeof(HttpRequestMessage));
            _exportedTypes.RemoveAll(contract => _ignoredTypes.Contains(contract));

            _container = new UnityContainer();            
        }

     

        [Test]
        public void UnityContainer_AfterRegistration_ContainsAllContracts()
        {
            RegisterAllDependencies(_container);
            foreach (var type in _exportedTypes)
            {
                Assert.That(_container.IsRegistered(type));
            } 
        }

        public static void RegisterAllDependencies(IUnityContainer container)
        {
            container
                .RegisterDependency<ApiServicesBootstrapper>()
                .RegisterDependency<DatabaseBootstrapper>()
                .RegisterType<IUrlLocatorConfig>();
        }
    }

}

// IUrlLocatorConfig,*
// ApiServicesBootstrapper,
// DatabaseBootstrapper,
// HttpRequestMessage, ---
// IUrlLocator,*
// ITodoListRepository*
// IBootstrapper*
