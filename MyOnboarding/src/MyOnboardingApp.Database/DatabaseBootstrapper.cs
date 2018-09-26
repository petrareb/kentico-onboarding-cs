using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Database.Repository;
using Unity;
using Unity.Lifetime;

namespace MyOnboardingApp.Database
{
    public class DatabaseBootstrapper: IBootstrapper
    {
        public void Register(IUnityContainer container) 
            => container.RegisterType<ITodoListRepository, TodoListRepository>(new HierarchicalLifetimeManager());
    }
}