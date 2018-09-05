using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
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
            new TodoListItem("Default Item", new Guid("b301b12e-6014-42cd-baad-37cee56fe932"));
        public static List<TodoListItem> Items = new List<TodoListItem>(){ DefaultItem };

        // GET: api/v{version}/TodoList
        public IEnumerable<TodoListItem> Get()
        {
            return Items;
        }
        //public async Task<IEnumerable<TodoListItem>> Get()
        //{
        //    return Items;
        //}

        // GET: api/v{version}/TodoList/5
        [Route("{id}")]
        public TodoListItem Get(Guid id)
        {
            return DefaultItem;
        }
        //public async Task<TodoListItem> Get(Guid id)
        //{
        //    return DefaultItem;
        //}

        //POST: api/v{version}/TodoList
        public void Post([FromBody]TodoListItem newItem)
        {
            Items.Add(newItem);
        }
        //public async Task<IHttpActionResult> Post([FromBody]TodoListItem newItem)
        //{
        //    Items.Add(newItem);
        //    return Created("api/v{version}/todolist", newItem);
        //}

        // PUT: api/v{version}/TodoList/5
        [Route("{id}")]
        public void Put(Guid id, [FromBody]TodoListItem item)
        {
            Items.ForEach(i =>
            {
                if (i.Id == id) i.Text = item.Text;
            });
        }
        //public async Task<IHttpActionResult> Put(Guid id, [FromBody]TodoListItem item)
        //{
        //    Items.ForEach(i =>
        //    {
        //        if (i.Id == id) i.Text = item.Text;
        //    });
        //    return Ok();
        //}

        // DELETE: api/v{version}/TodoList/5
        //[Route("{id}")]
        //public async Task Delete(Guid id)
        //{
        //    Items.RemoveAll(item => item.Id == id);
        //}
        [Route("{id}")]
        public void Delete(Guid id)
        {
            Items.RemoveAll(item => item.Id == id);
        }
    }
}
