using System;

namespace MyOnboardingApp.Contracts.UrlLocation
{
    public interface IUrlLocator
    {
        string GetTodoListItemUrl(Guid id);
    }
}