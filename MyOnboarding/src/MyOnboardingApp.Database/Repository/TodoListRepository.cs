using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;

namespace MyOnboardingApp.Database.Repository
{
    internal class TodoListRepository : ITodoListRepository
    {
        private const string CollectionName = "TodoList";
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TodoListItem> _databaseItems;
        

        public TodoListRepository(IDatabaseConnection databaseConnection)
        {
            _database = GetDatabaseFromConnection(databaseConnection);
            _databaseItems = LoadCollectionFromDatabase(CollectionName);
        }


        public async Task<IEnumerable<TodoListItem>> GetAllItemsAsync()
            => await _databaseItems
                .Find(FilterDefinition<TodoListItem>.Empty)
                .ToListAsync();


        public async Task<TodoListItem> GetItemByIdAsync(Guid id)
            => await _databaseItems
                .Find(item => item.Id == id)
                .SingleOrDefaultAsync(CancellationToken.None);


        public async Task<TodoListItem> AddNewItemAsync(TodoListItem newItem)
        {
            await _databaseItems.InsertOneAsync(newItem);
            return newItem;
        }


        public async Task<TodoListItem> ReplaceItemAsync(TodoListItem replacingItem)
            => await _databaseItems.FindOneAndReplaceAsync(i => i.Id == replacingItem.Id, replacingItem);


        public async Task<TodoListItem> DeleteItemAsync(Guid id)
            => await _databaseItems.FindOneAndDeleteAsync(item => item.Id == id);


        private static IMongoDatabase GetDatabaseFromConnection(IDatabaseConnection databaseConnection)
        {
            var connection = MongoUrl.Create(databaseConnection.GetDatabaseConnectionString);
            var client = new MongoClient(connection);

            return client.GetDatabase(connection.DatabaseName);
        }


        private IMongoCollection<TodoListItem> LoadCollectionFromDatabase(string collectionName)
            => _database.GetCollection<TodoListItem>(collectionName);
    }
}