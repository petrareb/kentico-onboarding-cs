using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.TestUtils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Services
{
    [TestFixture]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class CreateItemServiceTests
    {
        private ICreateItemService _service;
        private readonly ITodoListRepository _repository = Substitute.For<ITodoListRepository>();
        private readonly IIdGenerator<Guid> _idGenerator = Substitute.For<IIdGenerator<Guid>>();
        private readonly IDateTimeGenerator _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
        private readonly IValidator<TodoListItem> _validator = Substitute.For<IValidator<TodoListItem>>();


        [SetUp]
        public void SetUp()
            => _service = new CreateItemService(_repository, _idGenerator, _dateTimeGenerator, _validator);


        [Test]
        public async Task AddNewItemAsync_ValidItemSpecified_ReturnsAddedItemWithNoError()
        {
            var itemToAdd = new TodoListItem
            {
                Id = Guid.Empty,
                Text = "Item to Test",
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var expectedId = new Guid("00000000-0000-0000-0000-000000000007");
            _idGenerator
                .GetNewId()
                .Returns(expectedId);
            var expectedDateTime = new DateTime(2018, 10, 10);
            _dateTimeGenerator
                .GetCurrentDateTime()
                .Returns(expectedDateTime);
            var completedItem = new TodoListItem
            {
                Id = expectedId,
                Text = itemToAdd.Text,
                CreationTime = expectedDateTime,
                LastUpdateTime = expectedDateTime
            };
            _validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == expectedId))
                .Returns(ItemWithErrors.Create(completedItem, new List<string>()));
            _repository
                .AddNewItemAsync(Arg.Is<TodoListItem>(arg => arg.Id == expectedId))
                .Returns(completedItem);

            var result = await _service.AddNewItemAsync(itemToAdd);

            Assert.That(result.Item, Is.EqualTo(completedItem).UsingItemEqualityComparer());
            Assert.That(result.WasOperationSuccessful, Is.True);
        }


        [Test]
        public async Task AddNewItemAsync_ItemWithInvalidTextSpecified_ReturnsItemWithErrors()
        {
            var itemToAdd = new TodoListItem
            {
                Id = Guid.Empty,
                Text = "     ",
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            _validator
                .Validate(Arg.Is<TodoListItem>(arg => arg.Id == itemToAdd.Id))
                .Returns(ItemWithErrors.Create(itemToAdd, new List<string> { "Error" }));

            var result = await _service.AddNewItemAsync(itemToAdd);

            Assert.That(result.Item, Is.EqualTo(itemToAdd).UsingItemEqualityComparer());
            Assert.That(result.WasOperationSuccessful, Is.False);
        }
    }
}