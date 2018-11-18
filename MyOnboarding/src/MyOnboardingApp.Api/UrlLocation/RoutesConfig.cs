using MyOnboardingApp.Contracts.Urls;

namespace MyOnboardingApp.Api.UrlLocation
{
    internal class RoutesConfig: IRoutesConfig
    {
        public const string TodoListItemRouteName = "ListItemUrl";
        public string TodoListItemRouteNameGetter => TodoListItemRouteName;
    }
}