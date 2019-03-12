using System;
using System.Collections.Generic;
using System.Linq;
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
            var (_, _, expectedResult) = ItemVariantsFactory
                .CreateItemVariants(
                    expectedId,
                    itemToAdd.Text,
                    expectedDateTime);
            validator
                .Validate(Arg.Any<TodoListItem>())
                .Returns(callInfo 
                    => ItemWithErrors.Create(callInfo.Arg<TodoListItem>(), Enumerable.Empty<Error>()));
            repository
                .AddNewItemAsync(Arg.Any<TodoListItem>())
                .Returns(callInfo => callInfo.Arg<TodoListItem>());

            var result = await service.AddNewItemAsync(itemToAdd);

            Assert.Multiple(async () =>
            {
                await repository
                    .Received(1)
                    .AddNewItemAsync(Arg.Any<TodoListItem>());
                dateTimeGenerator
                    .Received(1)
                    .GetCurrentDateTime();
                idGenerator
                    .Received(1)
                    .GetNewId();
                Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
            });
        }


        [Test]
        public async Task AddNewItemAsync_ItemWithInvalidTextSpecified_ReturnsItemWithErrors()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var idGenerator = Substitute.For<IIdGenerator<Guid>>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new CreateItemService(repository, idGenerator, dateTimeGenerator, validator);

            var error = new Error(ErrorCode.DataValidationError, "error happened", "text");
            var errors = new [] { error };
            var (itemToAdd, _, validatedItem) = ItemVariantsFactory
                .CreateItemVariants(
                    Guid.Empty,
                    "     ",
                    DateTime.MinValue,
                    errors
                );
            
            validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == itemToAdd.Id))
                .Returns(validatedItem);
           
            var result = await service.AddNewItemAsync(itemToAdd);

            Assert.Multiple(async () =>
            {
                await repository
                    .Received(0)
                    .AddNewItemAsync(itemToAdd);
                Assert.That(result, Is.EqualTo(validatedItem).UsingItemWithErrorsEqualityComparer());
            });
        }


        [Test]
        public void AddNewItemAsync_NullItemSpecified_ThrowsArgumentNullException()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var idGenerator = Substitute.For<IIdGenerator<Guid>>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new CreateItemService(repository, idGenerator, dateTimeGenerator, validator);

            Assert.Multiple(async () =>
            {
                await repository
                    .Received(0)
                    .AddNewItemAsync(null);

                Assert.ThrowsAsync<ArgumentNullException>(
                    () => service.AddNewItemAsync(null));
            });
        }
    }
}