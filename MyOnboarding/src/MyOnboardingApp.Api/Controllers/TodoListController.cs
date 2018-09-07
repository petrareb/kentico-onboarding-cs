using System;
using System.Collections.Generic;
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
        public static TodoListItem DefaultItem =
            new TodoListItem{ Text = "Default Item", Id = new Guid("b301b12e-6014-42cd-baad-37cee56fe932") };
        public static List<TodoListItem> Items = new List<TodoListItem>{ DefaultItem };


        public TodoListController()
        {
            Request = new HttpRequestMessage();
            Configuration = new HttpConfiguration();
        }

        // GET: api/v{version}/TodoList
        public async Task<IHttpActionResult> GetAsync()
        {
            return await Task.FromResult((IHttpActionResult)Ok(Items));
        }

        // GET: api/v{version}/TodoList/5
        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            return await Task.FromResult((IHttpActionResult)Ok(DefaultItem));
        }

        //POST: api/v{version}/TodoList
        public async Task<IHttpActionResult> PostAsync([FromBody]TodoListItem newItem)
        {
            Items.Add(newItem);
            return await Task.FromResult((IHttpActionResult)Created("api/v{version}/todolist", newItem));
        }

        // PUT: api/v{version}/TodoList/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody]TodoListItem item)
        {
            Items.ForEach(i =>
            {
                if (i.Id == id) i.Text = item.Text;
            });
            return await Task.FromResult((IHttpActionResult)Ok());
        }

        // DELETE: api/v{version}/TodoList/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await Task.FromResult(Items.RemoveAll(item => item.Id == id));
            return await Task.FromResult((IHttpActionResult)Ok());
        }
    }
}
