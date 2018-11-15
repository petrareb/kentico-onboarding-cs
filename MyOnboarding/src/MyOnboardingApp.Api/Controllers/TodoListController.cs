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
        private readonly ICreateItemService _createService;


        public TodoListController(ITodoListRepository repository, IUrlLocator urlLocator, IRetrieveItemService retrieveService, ICreateItemService createService)
        {
            _repository = repository;
            _urlLocator = urlLocator;
            _retrieveService = retrieveService;
            _createService = createService;
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
            ValidateItemBeforePost(newItem);
            if (!ModelState.IsValid)
            {
                return BadRequest("Properties of given item are not valid.");
            }

            var storedItem = await _createService.AddNewItemAsync(newItem);
            if (!storedItem.WasOperationSuccessful)

            {
                return BadRequest("It was impossible to store the item.");
            }

            var location = _urlLocator.GetListItemUrl(storedItem.Item.Id);
            return Created(location, storedItem.Item);
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


        private void ValidateIdPost(Guid id)
        {
            if (id != Guid.Empty)
            {
                ModelState.AddModelError("Id", "Id must be empty.");
            }
        }


        private void ValidateTimePost(DateTime time, string field)
        {
            if (time != DateTime.MinValue)
            {
                ModelState.AddModelError(field, "Time is supposed to be DateTime.MinValue.");
            }
        }


        private void ValidateNotEmptyText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ModelState.AddModelError("Text", "Text must not be empty.");
            }
        }


        private void ValidateItemBeforePost(TodoListItem item)
        {
            ValidateNotEmptyText(item.Text);
            ValidateTimePost(item.CreationTime, "CreationTime");
            ValidateTimePost(item.LastUpdateTime, "LastUpdateTime");
            ValidateIdPost(item.Id);
        }
    }


}
