using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Tests.Comparators;

namespace MyOnboardingApp.Tests.Utils
{
    public static class ControllerExtensions
    {
        public static async Task<HttpResponseMessage> GetMessageFromActionAsync<T>(this T controller, Func<T, Task<IHttpActionResult>> action)
        {
            var result = await action(controller);
            return await result.ExecuteAsync(CancellationToken.None);
        }

        //public static bool CheckTodoListItemsEquality<T>(this T controller, TodoListItem x, TodoListItem y)
        //{
        //    var comparer = new ItemEqualityComparer();
        //    return comparer.Equals(x, y);
        //}
    }
}