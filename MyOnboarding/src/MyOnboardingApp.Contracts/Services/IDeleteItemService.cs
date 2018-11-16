using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Services
{
    public interface IDeleteItemService
    {
        Task<IResolvedItem<TodoListItem>> DeleteItemAsync(Guid id);
    }
}