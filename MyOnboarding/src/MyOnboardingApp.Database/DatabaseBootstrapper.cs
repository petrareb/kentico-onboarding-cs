using System.Web.Http;
using MyOnboardingApp.Content.Repository;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Database.Repository
{
    public static class DatabaseBootstrapper
    {
        public static void Register(UnityContainer container, HttpConfiguration config)
        {
            container.RegisterType<ITodoListRepository, TodoListRepository>(new HierarchicalLifetimeManager());
        }
    }
}