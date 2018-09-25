using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyOnboardingApp.ApiServices;
using MyOnboardingApp.Database;
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class ContainerDependenciesTest
    {
        private readonly IUnityContainer _container = Substitute.For<IUnityContainer>();

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void UnityContainer_AfterRegistration_ContainsAllContracts()
        {
            var assembly = Assembly.Load("MyOnboardingApp.Contracts");
            var ignoredContracts = new List<Type> { typeof(DatabaseBootstrapper), typeof(ApiServicesBootstrapper) };

            var contracts = assembly.GetTypes().Where(t => !ignoredContracts.Contains(t))/*.Where(type => type.IsInterface).*/.ToList();

            foreach (Type t in contracts)
            {
                //_container.RegisterDependency<t>();
            }

            foreach (Type t in contracts)
            {
                Assert.That(_container.IsRegistered(t));
            }

            //IUrlLocatorConfig*
            //    HttpRequestMessage
            //        IUrlLocator*
            //            ITodoListRepository*

        }
    }

}
