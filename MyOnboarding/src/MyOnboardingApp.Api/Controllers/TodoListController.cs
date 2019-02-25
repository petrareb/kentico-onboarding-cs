using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Web.Http;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Urls;
using MyOnboardingApp.Contracts.Validation;

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
            ValidateIdPresence(id);
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
            ValidateItemBeforeCreating(newItem);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storedItem = await _createService.AddNewItemAsync(newItem);
            if (!storedItem.WasOperationSuccessful)
            {
                return BadRequest(storedItem);
            }

            var location = _urlLocator.GetListItemUrl(storedItem.Item.Id);
            return Created(location, storedItem.Item);
        }


        [Route("{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, [FromBody] TodoListItem replacingItem)
        {
            ValidateItemBeforeEditing(replacingItem, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingItem = await _retrieveService.GetItemByIdAsync(replacingItem.Id);
            if (!existingItem.WasOperationSuccessful)
            {
                var itemToAdd = CreateItemWithTextOnly(replacingItem);
                return await PostAsync(itemToAdd);
            }
            
            var editedItem = await _editService.EditItemAsync(replacingItem, existingItem);
            if (!editedItem.WasOperationSuccessful)
            {
                return BadRequest(editedItem);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            ValidateIdPresence(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedItemWithStatus = await _deleteService.DeleteItemAsync(id);
            if (!deletedItemWithStatus.WasOperationSuccessful)
            {
                return NotFound();
            }

            return Ok(deletedItemWithStatus.Item);
        }


        private InvalidModelStateResult BadRequest(IItemWithErrors<TodoListItem> item)
        {
            foreach (var error in item.Errors)
            {
                ModelState.AddModelError(error.Location, error.Message);
            }

            return BadRequest(ModelState);
        }


        private static TodoListItem CreateItemWithTextOnly(TodoListItem item)
            => new TodoListItem
            {
                Text = item.Text,
            };


        private void ValidateIdPresence(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), "Identifier must not be empty.");
            }
        }


        private void ValidateIdAbsence(Guid id)
        {
            if (id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), "Identifier must be empty.");
            }
        }


        private void ValidateDateTimeSet(DateTime time, string field)
        {
            if (time != DateTime.MinValue)
            {
                ModelState.AddModelError(field, $"Time is supposed to have a value of {DateTime.MinValue}.");
            }
        }


        private void ValidateNotEmptyText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ModelState.AddModelError(nameof(TodoListItem.Text), "Text must not be empty.");
            }
        }


        private void ValidateItemBeforeCreating(TodoListItem item)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(TodoListItem), "Item from the request body must not be null.");
                return;
            }

            ValidateNotEmptyText(item.Text);
            ValidateDateTimeSet(item.CreationTime, nameof(TodoListItem.CreationTime));
            ValidateDateTimeSet(item.LastUpdateTime, nameof(TodoListItem.LastUpdateTime));
            ValidateIdAbsence(item.Id);
        }


        private void ValidateItemBeforeEditing(TodoListItem item, Guid idFromUrl)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(TodoListItem), "Item from the request body must not be null.");
                return;
            }

            if (item.Id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(TodoListItem.Id), "Identifier must not be empty.");
            }

            if (item.Id != idFromUrl)
            {
                ModelState.AddModelError(nameof(TodoListItem.Id), "Identifier in the body of request must be the same as in the URL.");
            }

            ValidateNotEmptyText(item.Text);
        }
    }
}
