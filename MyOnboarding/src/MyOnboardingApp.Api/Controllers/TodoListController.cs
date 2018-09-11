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
        private readonly ITodoListRepository _repository;

        public TodoListController(ITodoListRepository repository)
        {
            _repository = repository;
            Request = new HttpRequestMessage();
            Configuration = new HttpConfiguration();
        }

        public async Task<IHttpActionResult> GetAsync() =>
            await Task.FromResult(Ok(await _repository.GetAllItemsAsync()));


        [Route("{id}")]
        public async Task<IHttpActionResult> GetAsync(Guid id) =>
            await Task.FromResult(Ok(await _repository.GetItemByIdAsync(id)));


        public async Task<IHttpActionResult> PostAsync([FromBody] TodoListItem newItem) =>
            await Task.FromResult(Created("api/v{version}/todolist", await _repository.AddNewItemAsync(newItem)));
            

        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem item)
        {
            await _repository.EditItemAsync(id, item);
            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }
            

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id) =>
            await Task.FromResult(Ok(await _repository.DeleteItemAsync(id)));
    }
}
