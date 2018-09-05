using System;
using System.Collections.Generic;
using System.Linq;
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
            new TodoListItem("Default Item", new Guid("b301b12e-6014-42cd-baad-37cee56fe932"));
        public static List<TodoListItem> Items = new List<TodoListItem>(){ DefaultItem };

        // GET: api/v{version}/TodoList
        public IEnumerable<TodoListItem> Get()
        {
            return Items;
        }

        // GET: api/v{version}/TodoList/5
        [Route("{id}")]
        public TodoListItem Get(Guid id)
        {
            return DefaultItem;
        }

         //POST: api/v{version}/TodoList
        public IEnumerable<TodoListItem> Post([FromBody]TodoListItem newItem)
        {
            Items.Add(newItem);
            return Items;
        }

        // PUT: api/v{version}/TodoList/5
        [Route("{id}")]
        public IEnumerable<TodoListItem> Put(Guid id, [FromBody]TodoListItem item)
        {
            foreach (var i in Items)
            {
                if (i.Id == id)
                {
                    i.Text = item.Text;
                }
            }
            return Items;
        }

        // DELETE: api/v{version}/TodoList/5
        [Route("{id}")]
        public IEnumerable<TodoListItem> Delete(Guid id)
        {
            Items.RemoveAll(item => item.Id == id);
            return Items;
        }
    }
}
