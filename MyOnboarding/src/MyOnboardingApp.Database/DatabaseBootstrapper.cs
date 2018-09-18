﻿using MyOnboardingApp.Contracts.Configuration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Database.Repository;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Database
{
    public class DatabaseBootstrapper: IConfiguration
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<ITodoListRepository, TodoListRepository>(new HierarchicalLifetimeManager());
        }

    }
}