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
        private readonly IUrlLocator _urlLocator;
        private readonly ITodoListRepository _repository;
        private readonly IRetrieveItemService _retrieveService;
        private readonly ICreateItemService _createService;
        private readonly IDeleteItemService _deleteService;
        private readonly IUpdateItemService _editService;


        public TodoListController(IUrlLocator urlLocator, ITodoListRepository repository, IRetrieveItemService retrieveService, ICreateItemService createService,
            IDeleteItemService deleteService, IUpdateItemService editService)
        {
            _urlLocator = urlLocator;
            _repository = repository;
            _retrieveService = retrieveService;
            _createService = createService;
            _deleteService = deleteService;
            _editService = editService;
        }


        public async Task<IHttpActionResult> GetAsync()
            => Ok(await _repository.GetAllItemsAsync());


        [Route("{id}", Name = RoutesConfig.TodoListItemRouteName)]
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            ValidateNonEmptyGuid(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
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
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem replacingItem)
        {
            ValidateItemBeforePut(replacingItem, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingItem = await _retrieveService.GetItemByIdAsync(replacingItem.Id);
            if (!existingItem.WasOperationSuccessful)
            {
                var itemToAdd = PrepareItemForCreation(replacingItem);
                return await PostAsync(itemToAdd);
            }

            var completedItem = CompleteItemBeforePut(replacingItem, existingItem.Item);
            var editedItem = await _editService.EditItemAsync(completedItem);

            // ReSharper disable once InvertIf
            if (!editedItem.WasOperationSuccessful)
            {
                foreach (var error in editedItem.Errors)
                {
                    ModelState.AddModelError(error.Location, error.Message);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            ValidateNonEmptyGuid(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedItemWithStatus = await _deleteService.DeleteItemAsync(id);
            if (!deletedItemWithStatus.WasOperationSuccessful)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return Ok(deletedItemWithStatus.Item);
        }


        private static TodoListItem PrepareItemForCreation(TodoListItem item)
        {
            item.Id = Guid.Empty;
            return item;
        }


        private TodoListItem CompleteItemBeforePut(TodoListItem replacingItem, TodoListItem existingItem)
        {
            replacingItem.CreationTime = existingItem.CreationTime;
            replacingItem.LastUpdateTime = existingItem.LastUpdateTime;
            return replacingItem;
        }


        private void ValidateNonEmptyGuid(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), "Id must not be empty.");
            }
        }


        private void ValidateIdPost(Guid id)
        {
            if (id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), "Id must be empty.");
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
                ModelState.AddModelError(nameof(text), "Text must not be empty.");
            }
        }


        private void ValidateItemBeforePost(TodoListItem item)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(item), "Item from the request body must not be null.");
                return;
            }

            ValidateNotEmptyText(item.Text);
            ValidateTimePost(item.CreationTime, "CreationTime");
            ValidateTimePost(item.LastUpdateTime, "LastUpdateTime");
            ValidateIdPost(item.Id);
        }


        private void ValidateItemBeforePut(TodoListItem item, Guid idFromUrl)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(item), "Item from the request body must not be null.");
                return;
            }

            if (item.Id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(item.Id), "Id must not be empty.");
            }

            if (item.Id != idFromUrl)
            {
                ModelState.AddModelError(nameof(item.Id), "Id in the body of request must be the same as in the url.");
            }

            ValidateNotEmptyText(item.Text);
        }
    }
}
