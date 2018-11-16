using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    internal class UpdateItemService : IUpdateItemService
    {
        private readonly IValidator<TodoListItem> _validator;
        private readonly ITodoListRepository _repository;
        private readonly IDateTimeGenerator _dateTimeGenerator;


        public UpdateItemService(ITodoListRepository repository, IDateTimeGenerator dateTimeGenerator, IValidator<TodoListItem> validator)
        {
            _dateTimeGenerator = dateTimeGenerator;
            _repository = repository;
            _validator = validator;
        }


        public async Task<IResolvedItem<TodoListItem>> EditItemAsync(TodoListItem item)
        {
            var itemToReplace = CompleteItemToReplace(item);
            var itemWithErrors = _validator.Validate(itemToReplace);
            if (!itemWithErrors.WasOperationSuccessful)
            {
                return itemWithErrors;
            }

            var result = await _repository.ReplaceItemAsync(itemWithErrors.Item);

            return ResolvedItem.Create(result);
        }


        private TodoListItem CompleteItemToReplace(TodoListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item to complete must not be null.");
            }

            return new TodoListItem
            {
                Id = item.Id,
                Text = item.Text,
                CreationTime = item.CreationTime,
                LastUpdateTime = _dateTimeGenerator.GetCurrentDateTime()
            };
        }
    }
}