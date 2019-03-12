using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.TestUtils.Factories
{
    using ItemVariantsTuple = Tuple<TodoListItem, IResolvedItem<TodoListItem>, IItemWithErrors<TodoListItem>>;

    public static class ItemVariantsFactory
    {
        public static ItemVariantsTuple CreateItemVariants(
            string id, string text, string creationTime = "1900-01-01 0:00", string lastUpdateTime = "1900-01-01 0:00", IEnumerable<Error> errors = null)
        {
            var identifier = Guid.Parse(id);
            var creation = creationTime.ParseToDateTime();
            var update = lastUpdateTime.ParseToDateTime();

            return CreateItemVariants(identifier, text, creation, update, errors);
        }


        public static ItemVariantsTuple CreateItemVariants(
            string id, string text, string creationTime = "1900-01-01 0:00", IEnumerable<Error> errors = null) 
                => CreateItemVariants(id, text, creationTime, creationTime, errors);


        public static ItemVariantsTuple CreateItemVariants(
            Guid id, string text, DateTime creationTime, IEnumerable<Error> errors = null)
                => CreateItemVariants(id, text, creationTime, creationTime, errors);


        public static ItemVariantsTuple CreateItemVariants(
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
            var itemWithErrors = ItemWithErrors.Create(item, errors);

            return new ItemVariantsTuple(item, resolvedItem, itemWithErrors);
        }


        private static DateTime ParseToDateTime(this string date)
            => string.IsNullOrWhiteSpace(date)
                ? DateTime.MinValue
                : DateTime.Parse(date);
    }
}