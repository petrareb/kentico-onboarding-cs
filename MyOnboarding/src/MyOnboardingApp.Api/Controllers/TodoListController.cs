using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Urls;

namespace MyOnboardingApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/todolist")]
    [Route("")]
    public class TodoListController : ApiController
    {
        private readonly ITodoListRepository _repository;
        private readonly IUrlLocator _urlLocator;
        private readonly IRetrieveItemService _retrieveService;


        public TodoListController(ITodoListRepository repository, IUrlLocator urlLocator, IRetrieveItemService retrieveService)
        {
            _repository = repository;
            _urlLocator = urlLocator;
            _retrieveService = retrieveService;
        }


        public async Task<IHttpActionResult> GetAsync()
            => Ok(await _retrieveService.GetAllItemsAsync());


        [Route("{id}", Name = RoutesConfig.TodoListItemRouteName)]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("Id", "Id must not be empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Arguments are not valid.");
            }

            var itemWithStatus = await _retrieveService.GetItemByIdAsync(id);
            if (!itemWithStatus.WasOperationSuccessful)
            {
                return NotFound();
            }

            return Ok(itemWithStatus.Item);
        }


        public async Task<IHttpActionResult> PostAsync([FromBody] TodoListItem newItem)
        {
            var storedItem = await _repository.AddNewItemAsync(newItem);
            var location = _urlLocator.GetListItemUrl(storedItem.Id);

            return Created(location, storedItem);
        } 
            
            
        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem item)
        {
            await _repository.ReplaceItemAsync(item);
            return StatusCode(HttpStatusCode.NoContent);
        }
            

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id) 
            => Ok(await _repository.DeleteItemAsync(id));
    }
}
