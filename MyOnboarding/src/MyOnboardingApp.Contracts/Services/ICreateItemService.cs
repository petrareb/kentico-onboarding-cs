using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface ICreateItemService
    {
        Task<IResolvedItem<TodoListItem>> AddNewItemAsync(TodoListItem newItem);
    }
}