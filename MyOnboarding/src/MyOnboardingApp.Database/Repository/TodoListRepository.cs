using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MyOnboardingApp.Contracts.Database;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;

namespace MyOnboardingApp.Database.Repository
{
    internal class TodoListRepository : ITodoListRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TodoListItem> _databaseItems;
        private const string CollectionName = "TodoItems";


        public TodoListRepository(IDatabaseConnection connectionString)
        {
            _database = GetDatabaseFromConnection(connectionString);
            _databaseItems = LoadCollectionFromDatabase(CollectionName);
        }


        public async Task<IEnumerable<TodoListItem>> GetAllItemsAsync()
        {
            var results = await _databaseItems.FindAsync(FilterDefinition<TodoListItem>.Empty);
            return results.ToEnumerable();
        }


        public async Task<TodoListItem> GetItemByIdAsync(Guid id)
            => await _databaseItems.Find(_ => _.Id == id).SingleOrDefaultAsync(CancellationToken.None);


        public async Task<TodoListItem> AddNewItemAsync(TodoListItem newItem)
        {
            await _databaseItems.InsertOneAsync(newItem);
            return newItem;
        }


        public async Task<TodoListItem> ReplaceItemAsync(TodoListItem item)
        {
            var filter = Builders<TodoListItem>.Filter.Eq(_ => _.Id, item.Id);
            var updateDefinition = Builders<TodoListItem>.Update.Set("Text", item.Text).Set("LastUpdateTime", item.LastUpdateTime);
            var itemToReplace = await _databaseItems.FindOneAndUpdateAsync(filter, updateDefinition);
            return itemToReplace;
        }


        public async Task<TodoListItem> DeleteItemAsync(Guid id)
            => await _databaseItems.FindOneAndDeleteAsync(_ => _.Id == id);


        private static IMongoDatabase GetDatabaseFromConnection(IDatabaseConnection connectionString)
        {
            var connection = MongoUrl.Create(connectionString.GetDatabaseConnectionString());
            var client = new MongoClient(connection);
            var databaseName = connection.DatabaseName;
            return client.GetDatabase(databaseName);
        }


        private IMongoCollection<TodoListItem> LoadCollectionFromDatabase(string collectionName)
            => _database.GetCollection<TodoListItem>(collectionName);
    }
}