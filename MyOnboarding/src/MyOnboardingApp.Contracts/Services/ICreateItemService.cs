using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Services
{
    public interface ICreateItemService
    {
        Task<IItemWithErrors<TodoListItem>> AddNewItemAsync(TodoListItem newItem);
    }
}