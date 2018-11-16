using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;

namespace MyOnboardingApp.Services.Services
{
    internal class DeleteItemService : IDeleteItemService
    {
        private readonly ITodoListRepository _repository;


        public DeleteItemService(ITodoListRepository repository)
            => _repository = repository;


        public async Task<IResolvedItem<TodoListItem>> DeleteItemAsync(Guid id)
        {
            var deletedItem = await _repository.DeleteItemAsync(id);
            return ResolvedItem.Create(deletedItem);
        }
    }
}