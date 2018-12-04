using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    public abstract class AbstractUpsertService
    {
        private readonly IInvariantValidator<TodoListItem> _invariantValidator;


        protected AbstractUpsertService(IInvariantValidator<TodoListItem> validator)
            => _invariantValidator = validator;


        protected abstract Task StoreToDatabase(TodoListItem completedItem);

        protected abstract (Guid id, DateTime creationTime, DateTime lastUpdateTime) GetGeneratedData(TodoListItem originalItem);


        protected async Task<IItemWithErrors<TodoListItem>> TryCompleteAndStoreItemAsync(TodoListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item to complete must not be null.");
            }

            var completedItem = TryCompleteItem(item);
            if (!completedItem.WasOperationSuccessful)
            {
                return completedItem;
            }

            await StoreToDatabase(completedItem.Item);

            return completedItem;
        }


        private IItemWithErrors<TodoListItem> TryCompleteItem(TodoListItem inputItem)
        {
            var validatedInputItem = _invariantValidator.Validate(inputItem);
            if (!validatedInputItem.WasOperationSuccessful)
            {
                return validatedInputItem;
            }

            var completeItem = CompleteItem(validatedInputItem.Item);
            var validatedCompleteItem = _invariantValidator.Validate(completeItem);
            return validatedCompleteItem;
        }


        private TodoListItem CompleteItem(TodoListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item to complete must not be null.");
            }

            var text = item.Text?.Trim();
            var (id, creationTime, lastUpdateTime) = GetGeneratedData(item);

            return new TodoListItem
            {
                Id = id,
                Text = text,
                CreationTime = creationTime,
                LastUpdateTime = lastUpdateTime
            };
        }
    }
}