using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Contracts.Services
{
    public interface ICreateItemService
    {
        Task<IItemWithErrors<TodoListItem>> AddNewItemAsync(TodoListItem newItem);
    }
}