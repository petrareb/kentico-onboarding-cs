using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Repository
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface ITodoListRepository
    {
        Task<IEnumerable<TodoListItem>> GetAllItemsAsync();

        Task<TodoListItem> GetItemByIdAsync(Guid id);

        Task<TodoListItem> AddNewItemAsync(TodoListItem newItem);

        Task<TodoListItem> ReplaceItemAsync(TodoListItem item);

        Task<TodoListItem> DeleteItemAsync(Guid id);
    }
}