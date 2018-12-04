using System;
using System.Collections.Generic;
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
    public class CreateItemServiceTests
    {
        [Test]
        public async Task AddNewItemAsync_ValidItemSpecified_ReturnsAddedItemWithNoError()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var idGenerator = Substitute.For<IIdGenerator<Guid>>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new CreateItemService(repository, idGenerator, dateTimeGenerator, validator);

            var itemToAdd = new TodoListItem
            {
                Id = Guid.Empty,
                Text = "Item to Test",
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var expectedId = new Guid("00000000-0000-0000-0000-000000000007");
            idGenerator
                .GetNewId()
                .Returns(expectedId);
            var expectedDateTime = new DateTime(2018, 10, 10);
            dateTimeGenerator
                .GetCurrentDateTime()
                .Returns(expectedDateTime);
            var completedItem = new TodoListItem
            {
                Id = expectedId,
                Text = itemToAdd.Text,
                CreationTime = expectedDateTime,
                LastUpdateTime = expectedDateTime
            };
            validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == itemToAdd.Id))
                .Returns(ItemWithErrors.Create(completedItem, new List<Error>()));
            validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == expectedId))
                .Returns(ItemWithErrors.Create(completedItem, new List<Error>()));
            repository
                .AddNewItemAsync(Arg.Is<TodoListItem>(arg => arg.Id == expectedId))
                .Returns(completedItem);
            var expectedResult = ItemVariantsFactory
                .CreateItemVariants(completedItem)
                .ItemWithErrors;

            var result = await service.AddNewItemAsync(itemToAdd);

            await repository
                .Received(1)
                .AddNewItemAsync(completedItem);
            dateTimeGenerator
                .Received(1)
                .GetCurrentDateTime();
            idGenerator
                .Received(1)
                .GetNewId();
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public async Task AddNewItemAsync_ItemWithInvalidTextSpecified_ReturnsItemWithErrors()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var idGenerator = Substitute.For<IIdGenerator<Guid>>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new CreateItemService(repository, idGenerator, dateTimeGenerator, validator);

            var itemToAdd = new TodoListItem
            {
                Id = Guid.Empty,
                Text = "     ",
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "text");
            var errors = new List<Error> { error };
            validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == itemToAdd.Id))
                .Returns(ItemVariantsFactory.CreateItemVariants(itemToAdd, errors).ItemWithErrors);
            var expectedResult = ItemVariantsFactory
                .CreateItemVariants(itemToAdd, errors)
                .ItemWithErrors;

            var result = await service.AddNewItemAsync(itemToAdd);

            await repository
                .Received(0)
                .AddNewItemAsync(itemToAdd);
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public async Task AddNewItemAsync_NullItemSpecified_ThrowsArgumentNullException()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var idGenerator = Substitute.For<IIdGenerator<Guid>>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new CreateItemService(repository, idGenerator, dateTimeGenerator, validator);

            await repository
                .Received(0)
                .AddNewItemAsync(null);

            Assert.ThrowsAsync<ArgumentNullException>(
                () => service.AddNewItemAsync(null));
        }
    }
}