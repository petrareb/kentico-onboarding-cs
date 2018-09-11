using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyOnboardingApp.Content.Models;
using MyOnboardingApp.Content.Repository;

namespace MyOnboardingApp.Database.Repository
{
    internal class TodoListRepository: ITodoListRepository
    {
        private static readonly TodoListItem s_defaultItem =
            new TodoListItem { Text = "Default Item", Id = Guid.Empty };
        private static readonly List<TodoListItem> s_items = new List<TodoListItem> { s_defaultItem };


        public async Task<IEnumerable<TodoListItem>> GetAllItemsAsync() =>
            await Task.FromResult(s_items);
        

        public async Task<TodoListItem> GetItemByIdAsync(Guid id) =>
            await Task.FromResult(s_defaultItem);
        

        public async Task<TodoListItem> AddNewItemAsync(TodoListItem newItem) =>
            await Task.FromResult(newItem);


        public async Task EditItemAsync(Guid id, TodoListItem item) { }


        public async Task<TodoListItem> DeleteItemAsync(Guid id) => 
            await Task.FromResult(s_defaultItem);
    }
}
