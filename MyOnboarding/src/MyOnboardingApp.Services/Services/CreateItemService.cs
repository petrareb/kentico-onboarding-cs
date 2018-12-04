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


        public async Task<IResolvedItem<TodoListItem>> AddNewItemAsync(TodoListItem newItem)
            => await TryCompleteAndStoreItemAsync(newItem);


        protected override async Task StoreToDatabase(TodoListItem completedItem)
            => await _repository.AddNewItemAsync(completedItem);


        protected override (Guid id, DateTime creationTime, DateTime lastUpdateTime) GetGeneratedData(TodoListItem originalItem)
        {
            var id = _idGenerator.GetNewId();
            var currentDateTime = _dateTimeGenerator.GetCurrentDateTime();

            return (id, currentDateTime, currentDateTime);
        }
    }
}