using System;

namespace MyOnboardingApp.Contracts.Urls
{
    public interface IUrlLocator
    {
        string GetListItemUrl(Guid id);
    }
}