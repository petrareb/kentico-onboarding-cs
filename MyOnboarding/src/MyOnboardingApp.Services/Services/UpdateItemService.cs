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

            if (existingItem == null || !existingItem.WasOperationSuccessful)
            {
                throw new ArgumentException("Existing item does not actually exist.", nameof(existingItem));
            }

            var initData = GetGeneratedData(existingItem);

            return await TryCompleteAndStoreItemAsync(replacingItem, initData);
        }


        protected override async Task StoreToDatabase(TodoListItem completedItem)
            => await _repository.ReplaceItemAsync(completedItem);


        private ItemInitializingData GetGeneratedData(IResolvedItem<TodoListItem> resolvedItem)
            => new ItemInitializingData(
                resolvedItem.Item.Id, 
                resolvedItem.Item.CreationTime, 
                lastModificationTime: _dateTimeGenerator.GetCurrentDateTime());
    }
}
