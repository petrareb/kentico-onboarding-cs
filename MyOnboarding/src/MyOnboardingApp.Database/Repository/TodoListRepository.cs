using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;

namespace MyOnboardingApp.Database.Repository
{
    internal class TodoListRepository: ITodoListRepository
    {
        private static readonly TodoListItem[] s_items = {
            new TodoListItem
            {
                Text = "1st Todo Item",
                Id = new Guid("11111111-1111-1111-1111-aabbccddeeff"),
                CreationTime = new DateTime(1995, 01, 01),
                LastUpdateTime = new DateTime(1996, 01, 01)
            },
            new TodoListItem
            {
                Text = "2nd Todo Item",
                Id = new Guid("22222222-2222-2222-2222-aabbccddeeff"),
                CreationTime = new DateTime(2000, 01, 01),
                LastUpdateTime = new DateTime(2001, 01, 01)
            },
            new TodoListItem
            {
                Text = "3rd Todo Item",
                Id = new Guid("33333333-3333-3333-3333-aabbccddeeff"),
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            }
        };


        public async Task<IEnumerable<TodoListItem>> GetAllItemsAsync() 
            => await Task.FromResult(s_items);
        

        public async Task<TodoListItem> GetItemByIdAsync(Guid id) 
            => await Task.FromResult(s_items[0]);
        

        public async Task<TodoListItem> AddNewItemAsync(TodoListItem newItem) 
            => await Task.FromResult(s_items[2]);


        public async Task<TodoListItem> ReplaceItemAsync(TodoListItem item)
            => await Task.FromResult(item);


        public async Task<TodoListItem> DeleteItemAsync(Guid id) 
            => await Task.FromResult(s_items[1]);
    }
}
