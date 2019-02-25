using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Registration;
using NSubstitute;
using Unity;
using Unity.Lifetime;
using Unity.Registration;

namespace MyOnboardingApp.TestUtils.Mocks
{
    public static class UnityContainerMock
    {
        public static (Bootstrap bootstrap, ICollection<Type> registeredTypes) CreateMock()
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