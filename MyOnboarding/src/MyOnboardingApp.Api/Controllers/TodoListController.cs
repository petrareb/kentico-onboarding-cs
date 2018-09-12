using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http;
using MyOnboardingApp.Content.Models;
using MyOnboardingApp.Content.Repository;

namespace MyOnboardingApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todolist")]
    [Route("")]
    public class TodoListController : ApiController
    {
        private readonly ITodoListRepository _repository;
        private readonly IUrlLocator _url;

        // TODO DI
        public TodoListController(ITodoListRepository repository, IUrlLocator url)
        {
            _repository = repository;
            _url = url;
        }

        public async Task<IHttpActionResult> GetAsync() 
            => Ok(await _repository.GetAllItemsAsync());


        [Route("{id}", Name = DummyUrlLocator.TodoListRouteName)]
        public async Task<IHttpActionResult> GetAsync(Guid id) 
            => Ok(await _repository.GetItemByIdAsync(id));


        public async Task<IHttpActionResult> PostAsync([FromBody] TodoListItem newItem)
        {
            var storedItem = await _repository.AddNewItemAsync(newItem);
            var location = _url.GetTodoListItemUrl(storedItem.Id);//"api/v{version}/todolist/{id}"; //Request.RequestUri.ToString();
            var location2 = this.Url.Route(DummyUrlLocator.TodoListRouteName, new {id = storedItem.Id});
            return Created(location, storedItem);
        } 
            
            

        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem item)
        {
            await _repository.EditItemAsync(id, item);
            return StatusCode(HttpStatusCode.NoContent);
        }
            

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id) 
            => Ok(await _repository.DeleteItemAsync(id));
    }

    public interface IUrlLocator
    {
        string GetTodoListItemUrl(Guid id);
    }

    public class DummyUrlLocator: IUrlLocator
    {
        private readonly UrlHelper _url;
        public const string TodoListRouteName = "myRoute";

        public DummyUrlLocator(UrlHelper url)
        {
            _url = url;
        }
        public string GetTodoListItemUrl(Guid id)
        {
            return _url.Route(TodoListRouteName, new { id });
        }
    }
}
