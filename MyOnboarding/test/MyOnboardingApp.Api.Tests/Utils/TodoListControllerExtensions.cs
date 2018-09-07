using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MyOnboardingApp.Api.Controllers;

namespace MyOnboardingApp.Tests.Utils
{
    public static class TodoListControllerExtensions
    {
        public static async Task<HttpResponseMessage> GetResponseFromAction(this TodoListController controller, Func<TodoListController, Task<IHttpActionResult>> action)
        {
            var response = await action(controller);
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}