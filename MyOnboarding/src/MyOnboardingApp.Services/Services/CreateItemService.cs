using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    internal class CreateItemService : AbstractUpsertService, ICreateItemService
    {
        private readonly ITodoListRepository _repository;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly IDateTimeGenerator _dateTimeGenerator;


        public CreateItemService(ITodoListRepository repository, IIdGenerator<Guid> idGenerator, IDateTimeGenerator dateTimeGenerator, IInvariantValidator<TodoListItem> validator)
            : base(validator)
        {
            _repository = repository;
            _idGenerator = idGenerator;
            _dateTimeGenerator = dateTimeGenerator;
        }


        public async Task<IItemWithErrors<TodoListItem>> AddNewItemAsync(TodoListItem newItem)
        {
            var initializingData = GetGeneratedData();
            return await TryCompleteAndStoreItemAsync(newItem, initializingData);
        }


        protected override async Task StoreToDatabase(TodoListItem completedItem)
            => await _repository.AddNewItemAsync(completedItem);


        private ItemInitializingData GetGeneratedData()
        {
            var id = _idGenerator.GetNewId();
            var currentDateTime = _dateTimeGenerator.GetCurrentDateTime();

            return new ItemInitializingData(
                id, 
                creationTime: currentDateTime, 
                lastModificationTime: currentDateTime);
        }
    }
}