using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IUpdateItemService
    {
        Task<IResolvedItem<TodoListItem>> EditItemAsync(TodoListItem item);
    }
}