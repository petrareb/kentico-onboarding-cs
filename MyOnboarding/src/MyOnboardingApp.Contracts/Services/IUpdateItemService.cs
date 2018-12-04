using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Contracts.Services
{
    public interface IUpdateItemService
    {
        Task<IItemWithErrors<TodoListItem>> EditItemAsync(TodoListItem replacingItem, IResolvedItem<TodoListItem> existingItem);
    }
}