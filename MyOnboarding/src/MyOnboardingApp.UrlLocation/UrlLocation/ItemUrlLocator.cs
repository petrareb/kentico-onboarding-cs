using System;
using System.Web.Http.Routing;
using MyOnboardingApp.Contracts.UrlLocation;

namespace MyOnboardingApp.ApiServices.UrlLocation
{
    internal class ItemUrlLocator: IUrlLocator
    {
        private readonly UrlHelper _url;
        private const string TodoListRouteName = "ListItemUrl";

        public ItemUrlLocator(UrlHelper url)
        {
            _url = url;
        }

        public string GetListItemUrl(Guid id) => _url.Route(TodoListRouteName, new { id });


    }
}