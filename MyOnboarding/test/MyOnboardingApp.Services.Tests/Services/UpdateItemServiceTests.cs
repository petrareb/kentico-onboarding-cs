using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Services
{
    [TestFixture]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class UpdateItemServiceTests
    {
        [Test]
        public async Task EditItemAsync_ExistingItemSpecified_ReturnsEditedItemWithNoErrors()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new UpdateItemService(repository, dateTimeGenerator, validator);

            var updatedTime = new DateTime(2018, 10, 10);
            dateTimeGenerator
                .GetCurrentDateTime()
                .Returns(updatedTime);
            var itemToEdit = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "new text",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var expectedItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "new text",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = updatedTime
            };
            repository.ReplaceItemAsync(Arg.Is<TodoListItem>(arg => arg.Id == itemToEdit.Id)).Returns(expectedItem);
            validator.Validate(Arg.Is<TodoListItem>(item => item.Id == expectedItem.Id))
                .Returns(ItemWithErrors.Create(expectedItem, new List<Error>().AsReadOnly()));

            var resultItem = await service.EditItemAsync(itemToEdit);

            await repository.Received(1).ReplaceItemAsync(expectedItem);
            Assert.That(resultItem.Item, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
            Assert.That(resultItem.WasOperationSuccessful, Is.True);
        }


        [Test]
        public async Task EditItemAsync_InvalidItemSpecified_ReturnsItemWithErrors()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new UpdateItemService(repository, dateTimeGenerator, validator);
            var updatedTime = new DateTime(2018, 10, 10);
            dateTimeGenerator
                .GetCurrentDateTime()
                .Returns(updatedTime);
            var itemToEdit = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = string.Empty,
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "message", "location");
            var errors = new List<Error> { error };
            var itemWithError = ItemVariantsFactory.CreateItemVariants(itemToEdit, errors).ItemWithErrors;
            validator.Validate(itemToEdit).Returns(itemWithError);

            var result = await service.EditItemAsync(itemToEdit);

            await repository.Received(0).ReplaceItemAsync(itemToEdit);
            Assert.That(result.WasOperationSuccessful, Is.False);
            Assert.That(result.Errors, Is.EqualTo(errors));
        }


        [Test]
        public async Task EditItemAsync_NullItemSpecified_ThrowsArgumentNullException()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new UpdateItemService(repository, dateTimeGenerator, validator);

            await repository.Received(0).ReplaceItemAsync(null);
            Assert.ThrowsAsync<ArgumentNullException>(
                () => service.EditItemAsync(null));
        }
    }
}