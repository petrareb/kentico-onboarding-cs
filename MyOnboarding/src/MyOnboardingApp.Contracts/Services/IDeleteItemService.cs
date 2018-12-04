using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Contracts.Services
{
    public interface IDeleteItemService
    {
        Task<IResolvedItem<TodoListItem>> DeleteItemAsync(Guid id);
    }
}