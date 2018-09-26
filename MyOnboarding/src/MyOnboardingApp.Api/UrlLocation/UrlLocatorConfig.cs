using MyOnboardingApp.Contracts.Urls;

namespace MyOnboardingApp.Api.UrlLocation
{
    public class UrlLocatorConfig: IUrlLocatorConfig
    {
        public const string TodoListItemRouteName = "ListItemUrl";
        public string TodoListItemRouteNameGetter => TodoListItemRouteName;
    }
}