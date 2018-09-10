using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using MyOnboardingApp.Api.Models;

namespace MyOnboardingApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todolist")]
    [Route("")]
    public class TodoListController : ApiController
    {
        private static readonly TodoListItem s_defaultItem =
            new TodoListItem { Text = "Default Item", Id = Guid.Empty };
        private static readonly List<TodoListItem> s_items = new List<TodoListItem> { s_defaultItem };


        public TodoListController()
        {
            Request = new HttpRequestMessage();
            Configuration = new HttpConfiguration();
        }

        public async Task<IHttpActionResult> GetAsync() => 
            await Task.FromResult(Ok(s_items));


        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(Guid id) => 
            await Task.FromResult(Ok(s_defaultItem));


        public async Task<IHttpActionResult> PostAsync([FromBody]TodoListItem newItem) => 
            await Task.FromResult(Created("api/v{version}/todolist", newItem));


        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody]TodoListItem item) =>
            await Task.FromResult(StatusCode(HttpStatusCode.NoContent));


        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id) =>
            await Task.FromResult(Ok(s_defaultItem));
    }
}
