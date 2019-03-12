using System;
using System.Diagnostics.CodeAnalysis;
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
            var itemId = new Guid("00000000-0000-0000-0000-000000000001");
            var (_, existingResolvedItem, _) = ItemVariantsFactory.CreateItemVariants(
                itemId,
                "old text",
                new DateTime(2015, 01, 01),
                new DateTime(2015, 02, 02)
            );

            var (newItem, _, _) = ItemVariantsFactory.CreateItemVariants(
                itemId,
                "new text");

            var (_, _, expectedResult) = ItemVariantsFactory.CreateItemVariants(
                itemId,
                newItem.Text,
                existingResolvedItem.Item.CreationTime,
                updatedTime);

           
            repository
                .ReplaceItemAsync(Arg.Any<TodoListItem>())
                .Returns(callInfo => callInfo.Arg<TodoListItem>());
            validator
                .Validate(Arg.Any<TodoListItem>())
                .Returns(callInfo => ItemWithErrors.Create(callInfo.Arg<TodoListItem>(), Enumerable.Empty<Error>()));

            var resultItem = await service.EditItemAsync(newItem, existingResolvedItem);

            Assert.Multiple(async () =>
            {
                Assert.That(resultItem, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
                await repository
                    .Received(1)
                    .ReplaceItemAsync(Arg.Any<TodoListItem>());
            });
        }


        [Test]
        public void EditItemAsync_NullItemSpecified_ThrowsArgumentNullException()
        {
            var repository = Substitute.For<ITodoListRepository>();
            var dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            var validator = Substitute.For<IInvariantValidator<TodoListItem>>();
            var service = new UpdateItemService(repository, dateTimeGenerator, validator);

            Assert.Multiple(async () =>
            {
                await repository.Received(0).ReplaceItemAsync(null);
                Assert.ThrowsAsync<ArgumentNullException>(
                    () => service.EditItemAsync(null, Arg.Any<IResolvedItem<TodoListItem>>()));
            });
        }
    }
}