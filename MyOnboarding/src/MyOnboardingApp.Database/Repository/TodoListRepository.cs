using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;

namespace MyOnboardingApp.Database.Repository
{
    internal class TodoListRepository: ITodoListRepository
    {
        private static readonly TodoListItem s_defaultItem =
            new TodoListItem { Text = "Default Item", Id = new Guid("00112233-4455-6677-8899-aabbccddeeff") };
        private static readonly List<TodoListItem> s_items = new List<TodoListItem> { s_defaultItem };


        public async Task<IEnumerable<TodoListItem>> GetAllItemsAsync() 
            => await Task.FromResult(s_items);
        

        public async Task<TodoListItem> GetItemByIdAsync(Guid id) 
            => await Task.FromResult(s_defaultItem);
        

        public async Task<TodoListItem> AddNewItemAsync(TodoListItem newItem) 
            => await Task.FromResult(newItem);


        public async Task<TodoListItem> ReplaceItemAsync(TodoListItem item)
            => await Task.FromResult(item);


        public async Task<TodoListItem> DeleteItemAsync(Guid id) 
            => await Task.FromResult(s_defaultItem);
    }
}
