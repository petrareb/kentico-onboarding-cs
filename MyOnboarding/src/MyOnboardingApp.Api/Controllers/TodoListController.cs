using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        private ITodoListRepository _repository;

        public TodoListController(ITodoListRepository repository)
        {
            _repository = repository;
            Request = new HttpRequestMessage();
            Configuration = new HttpConfiguration();
        }

        public async Task<IHttpActionResult> GetAsync()
        {
            var items = await _repository.GetAllItemsAsync();
            return await Task.FromResult(Ok(items));
        }


        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var item = await _repository.GetItemByIdAsync(id);
            return await Task.FromResult(Ok(item));
        }


        public async Task<IHttpActionResult> PostAsync([FromBody] TodoListItem newItem)
        {
            var item = await _repository.AddNewItemAsync(newItem);
            return await Task.FromResult(Created("api/v{version}/todolist", item));
        }
            

        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem item)
        {
            await _repository.EditItemAsync(id, item);
            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }
            

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            var item = await _repository.DeleteItemAsync(id);
            return await Task.FromResult(Ok(item));
        }
            
    }
}
