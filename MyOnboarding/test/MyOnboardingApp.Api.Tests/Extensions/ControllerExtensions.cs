using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyOnboardingApp.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<HttpResponseMessage> GetMessageFromActionAsync<T>(this T controller, Func<T, Task<IHttpActionResult>> action)
        {
            var result = await action(controller);
            return await result.ExecuteAsync(CancellationToken.None);
        }
    }
}