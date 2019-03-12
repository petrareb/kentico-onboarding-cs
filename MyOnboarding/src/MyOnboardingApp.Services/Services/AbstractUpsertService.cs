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


        protected async Task<IItemWithErrors<TodoListItem>> TryCompleteAndStoreItemAsync(TodoListItem changingItem, ItemInitializingData initializingData)
        {
            if (changingItem == null)
            {
                throw new ArgumentNullException(nameof(changingItem), "Item to complete must not be null.");
            }

            var completedItem = TryCompleteItem(changingItem, initializingData);
            if (!completedItem.WasOperationSuccessful)
            {
                return completedItem;
            }

            await StoreToDatabase(completedItem.Item);

            return completedItem;
        }
        

        private IItemWithErrors<TodoListItem> TryCompleteItem(TodoListItem changingItem, ItemInitializingData initializingData)
        {
            var completeItem = CompleteItem(changingItem, initializingData);
            var validatedCompleteItem = _invariantValidator.Validate(completeItem);
            return validatedCompleteItem;
        }


        private static TodoListItem CompleteItem(TodoListItem changingItem, ItemInitializingData initializingData)
        {
            var text = changingItem.Text?.Trim();

            return new TodoListItem
            {
                Id = initializingData.Id,
                Text = text,
                CreationTime = initializingData.CreationTime,
                LastUpdateTime = initializingData.LastUpdateTime
            };
        }


        protected struct ItemInitializingData
        {
            public Guid Id { get; }
            public DateTime CreationTime { get; }
            public DateTime LastUpdateTime { get; }


            public ItemInitializingData(Guid id, DateTime creationTime,  DateTime lastModificationTime)
            {
                Id = id;
                CreationTime = creationTime;
                LastUpdateTime = lastModificationTime;
            }
        }
    }
}