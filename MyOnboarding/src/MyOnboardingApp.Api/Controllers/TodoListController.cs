using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyOnboardingApp.Api.Entities;

namespace MyOnboardingApp.Api.Controllers
{
    public class TodoListController : ApiController
    {
        public List<TodoListItem> Items = new List<TodoListItem>();

        public TodoListController(List<TodoListItem> items = null)
        {
            if (items != null) {
                Items.AddRange(items);
            }
        }

  
        // GET: api/TodoList
        public IEnumerable<TodoListItem> Get()
        {
            return Items;
        }

        // GET: api/TodoList/5
        // should return default
        public TodoListItem Get(Guid id)
        {
            //return Items.Find(i => i.Id == id);
            return new TodoListItem("Default item");
        }

        // POST: api/TodoList
        // == ADD NEW!
        public void Post([FromBody]string text)
        {
            Items.Add(new TodoListItem(text));
        }

        // PUT: api/TodoList/5
        // == UPDATE!
        public void Put(Guid id, [FromBody]string newText)
        {
            //var item = Items.Find(i => i.Id == id);
            //if (item != null)
            //{
            //    item.Text = newText;
            //    Items.
            //} 
            Items.Where(item => item.Id == id).Select(item =>
            {
                item.Text = newText;
                return item;
            });
        }

        // DELETE: api/TodoList/5
        public void Delete(Guid id)
        {
            Items.RemoveAll(item => item.Id == id);
        }
    }
}
