using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Services.Services
{
    internal class RetrieveItemService : IRetrieveItemService
    {
        private readonly ITodoListRepository _repository;


        public RetrieveItemService(ITodoListRepository repository)
            => _repository = repository;


        public async Task<IResolvedItem<TodoListItem>> GetItemByIdAsync(Guid id)
        {
            var requestedItem = await _repository.GetItemByIdAsync(id);
            return ResolvedItem.Create(requestedItem);
        }
    }
}