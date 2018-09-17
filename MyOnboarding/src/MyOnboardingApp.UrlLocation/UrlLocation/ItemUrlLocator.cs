using System;
using System.Web.Http.Routing;
using MyOnboardingApp.Contracts.UrlLocation;

namespace MyOnboardingApp.ApiServices.UrlLocation
{
    public class ItemUrlLocator: IUrlLocator
    {
        private readonly UrlHelper _url;
        public const string TodoListRouteName = "myRoute";

        public ItemUrlLocator(UrlHelper url)
        {
            _url = url;
        }
        public string GetTodoListItemUrl(Guid id)
        {
            return _url.Route(TodoListRouteName, new { id });
        }
    }
}