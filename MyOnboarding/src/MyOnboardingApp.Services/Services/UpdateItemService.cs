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


        public async Task<IItemWithErrors<TodoListItem>> EditItemAsync(TodoListItem replacingItem)
            => await TryCompleteAndStoreItemAsync(replacingItem);


        protected override async Task StoreToDatabase(TodoListItem completedItem)
            => await _repository.ReplaceItemAsync(completedItem);


        protected override (Guid id, DateTime creationTime, DateTime lastUpdateTime) GetGeneratedData(TodoListItem originalItem)
            => (originalItem.Id, originalItem.CreationTime, _dateTimeGenerator.GetCurrentDateTime());
    }
}