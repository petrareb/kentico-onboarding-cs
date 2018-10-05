using System.Runtime.CompilerServices;
using MyOnboardingApp.Contracts.Urls;

[assembly: InternalsVisibleTo("MyOnboardingApp.ApiServices.Tests")]

namespace MyOnboardingApp.Api.UrlLocation
{
    internal class RoutesConfig: IRoutesConfig
    {
        public const string TodoListItemRouteName = "ListItemUrl";
        public string TodoListItemRouteNameGetter => TodoListItemRouteName;
    }
}