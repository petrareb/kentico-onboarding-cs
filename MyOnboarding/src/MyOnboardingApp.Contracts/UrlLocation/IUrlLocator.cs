using System;

namespace MyOnboardingApp.Contracts.UrlLocation
{
    public interface IUrlLocator
    {
        string GetListItemUrl(Guid id);
    }
}