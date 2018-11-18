using System;
using System.Web.Http.Routing;
using MyOnboardingApp.Contracts.Urls;

namespace MyOnboardingApp.ApiServices.UrlLocation
{
    internal class ItemUrlLocator: IUrlLocator
    {
        private readonly UrlHelper _url;
        private readonly IRoutesConfig _urlConfiguration;


        public ItemUrlLocator(UrlHelper url, IRoutesConfig config)
        {
            _url = url;
            _urlConfiguration = config;
        }


        public string GetListItemUrl(Guid id) 
            => _url.Route(_urlConfiguration.TodoListItemRouteNameGetter, new { id });
    }
}