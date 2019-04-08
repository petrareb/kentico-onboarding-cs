﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace MyOnboardingApp.Api.DependencyResolvers
{
    internal sealed class DependencyResolver: IDependencyResolver
    {
        private readonly IUnityContainer _container;


        public DependencyResolver(IUnityContainer container) 
            => _container = container ?? throw new ArgumentNullException(nameof(container));


        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }


        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return Enumerable.Empty<object>();
            }
        }


        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new DependencyResolver(child);
        }


        public void Dispose() 
            => _container?.Dispose();
    }
}