using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Factories
{
    public static class ItemVariantsFactory
    {
        public static (TodoListItem Item, IResolvedItem<TodoListItem> ResolvedItem, IItemWithErrors<TodoListItem> ItemWithErrors) CreateItemVariants(
            string id, string text, string creationTime = "1900-01-01 0:00", string lastUpdateTime = "1900-01-01 0:00", IEnumerable<Error> errors = null)
        {
            var identifier = Guid.Parse(id);
            var creation = creationTime.ParseToDateTime();
            var update = lastUpdateTime.ParseToDateTime();

            return CreateItemVariants(identifier, text, creation, update, errors);
        }


        public static (TodoListItem Item, IResolvedItem<TodoListItem> ResolvedItem, IItemWithErrors<TodoListItem>
            ItemWithErrors) CreateItemVariants(string id, string text, string creationTime = "1900-01-01 0:00", IEnumerable<Error> errors = null) 
                => CreateItemVariants(id, text, creationTime, creationTime, errors);


        public static (TodoListItem Item, IResolvedItem<TodoListItem> ResolvedItem, IItemWithErrors<TodoListItem> ItemWithErrors)
            CreateItemVariants(Guid id, string text, DateTime creationTime, IEnumerable<Error> errors = null)
                => CreateItemVariants(id, text, creationTime, creationTime, errors);


        public static (TodoListItem Item, IResolvedItem<TodoListItem> ResolvedItem, IItemWithErrors<TodoListItem> ItemWithErrors) CreateItemVariants(
            Guid id, string text, DateTime? creationTime = null, DateTime? lastUpdateTime = null, IEnumerable<Error> errors = null)
        {
            var item = new TodoListItem
            {
                Id = id,
                Text = text,
                CreationTime = creationTime.GetValueOrDefault(),
                LastUpdateTime = lastUpdateTime.GetValueOrDefault()
            };

            var resolvedItem = ResolvedItem.Create(item);

            var readonlyErrors = errors.MakeReadOnly();
            var itemWithErrors = ItemWithErrors.Create(item, readonlyErrors);

            return (item, resolvedItem, itemWithErrors);
        }


        public static (TodoListItem Item, IResolvedItem<TodoListItem> ResolvedItem, IItemWithErrors<TodoListItem>
            ItemWithErrors) CreateItemVariants(
                TodoListItem item,
                IEnumerable<Error> errors = null)
        {
            var resolvedItem = ResolvedItem.Create(item);

            var readonlyErrors = errors.MakeReadOnly();
            var itemWithErrors = ItemWithErrors.Create(item, readonlyErrors);

            return (item, resolvedItem, itemWithErrors);
        }


        private static IReadOnlyCollection<TItem> MakeReadOnly<TItem>(this IEnumerable<TItem> collection)
            => (collection ?? Enumerable.Empty<TItem>())
                .ToList()
                .AsReadOnly();


        private static DateTime ParseToDateTime(this string date)
            => string.IsNullOrWhiteSpace(date)
                ? DateTime.MinValue
                : DateTime.Parse(date);
    }
}