using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    internal class UpdateItemService : AbstractUpsertService, IUpdateItemService
    {
        private readonly ITodoListRepository _repository;
        private readonly IDateTimeGenerator _dateTimeGenerator;


        public UpdateItemService(ITodoListRepository repository, IDateTimeGenerator dateTimeGenerator, IInvariantValidator<TodoListItem> validator)
            : base(validator)
        {
            _dateTimeGenerator = dateTimeGenerator;
            _repository = repository;
        }


        public async Task<IItemWithErrors<TodoListItem>> EditItemAsync(TodoListItem replacingItem, IResolvedItem<TodoListItem> existingItem)
        {
            if (replacingItem == null)
            {
                throw new ArgumentNullException(nameof(replacingItem), "Item to complete must not be null.");
            }

            var item = CompleteItemAccordingToExisting(replacingItem, existingItem.Item);
            return await TryCompleteAndStoreItemAsync(item);
        }


        protected override async Task StoreToDatabase(TodoListItem completedItem)
            => await _repository.ReplaceItemAsync(completedItem);


        protected override (Guid id, DateTime creationTime, DateTime lastUpdateTime) GetGeneratedData(TodoListItem originalItem)
            => (originalItem.Id, originalItem.CreationTime, _dateTimeGenerator.GetCurrentDateTime());


        private static TodoListItem CompleteItemAccordingToExisting(TodoListItem replacingItem, TodoListItem existingItem)
        {
            replacingItem.CreationTime = existingItem.CreationTime;
            replacingItem.LastUpdateTime = existingItem.LastUpdateTime;
            return replacingItem;
        }
    }
}