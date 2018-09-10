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
        public static async Task<HttpResponseMessage> GetMessageFromAction(this TodoListController controller, Func<TodoListController, Task<IHttpActionResult>> action)
        {
            var result = await action(controller);
            return await result.ExecuteAsync(CancellationToken.None);
        }
    }
}