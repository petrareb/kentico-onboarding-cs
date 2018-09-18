using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;

namespace MyOnboardingApp.Contracts.Repository
{
    public interface ITodoListRepository
    {
        Task<IEnumerable<TodoListItem>> GetAllItemsAsync();

        Task<TodoListItem> GetItemByIdAsync(Guid id);

        Task<TodoListItem> AddNewItemAsync(TodoListItem newItem);

        Task EditItemAsync(Guid id, TodoListItem item);

        Task<TodoListItem> DeleteItemAsync(Guid id);
    }
}