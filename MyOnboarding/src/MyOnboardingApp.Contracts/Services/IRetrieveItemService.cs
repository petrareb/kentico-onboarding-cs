using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Contracts.Services
{
    public interface IRetrieveItemService
    {
        Task<IResolvedItem<TodoListItem>> GetItemByIdAsync(Guid id);
    }
}
