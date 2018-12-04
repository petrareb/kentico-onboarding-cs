using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    internal class CreateItemService : ICreateItemService
    {
        private readonly IInvariantValidator<TodoListItem> _validator;
        private readonly ITodoListRepository _repository;
        private readonly IIdGenerator<Guid> _idGenerator;
        private readonly IDateTimeGenerator _dateTimeGenerator;


        public CreateItemService(ITodoListRepository repository, IIdGenerator<Guid> idGenerator, IDateTimeGenerator dateTimeGenerator, IInvariantValidator<TodoListItem> validator)
        {
            _repository = repository;
            _idGenerator = idGenerator;
            _dateTimeGenerator = dateTimeGenerator;
            _validator = validator;
        }


        public async Task<IResolvedItem<TodoListItem>> AddNewItemAsync(TodoListItem newItem)
        {
            var itemToAdd = CompleteItemToAdd(newItem);
            var itemWithErrors = _validator.Validate(itemToAdd);
            if (!itemWithErrors.WasOperationSuccessful)
            {
                return itemWithErrors;
            }

            var result = await _repository.AddNewItemAsync(itemWithErrors.Item);

            return ResolvedItem.Create(result);
        }


        private TodoListItem CompleteItemToAdd(TodoListItem item)
        {
            var currentDateTime = _dateTimeGenerator.GetCurrentDateTime();
            return new TodoListItem
            {
                Text = item.Text,
                Id = _idGenerator.GetNewId(),
                CreationTime = currentDateTime,
                LastUpdateTime = currentDateTime
            };
        }
    }
}