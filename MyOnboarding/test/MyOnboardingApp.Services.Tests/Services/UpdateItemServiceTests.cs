using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.TestUtils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Services
{
    [TestFixture]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class UpdateItemServiceTests
    {
        private UpdateItemService _service;
        private IDateTimeGenerator _dateTimeGenerator;
        private ITodoListRepository _repository;
        private IValidator<TodoListItem> _validator;


        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<ITodoListRepository>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            _validator = Substitute.For<IValidator<TodoListItem>>();
            _service = new UpdateItemService(_repository, _dateTimeGenerator, _validator);
        }


        [Test]
        public async Task EditItemAsync_ExistingItemSpecified_ReturnsEditedItemWithNoErrors()
        {
            var updatedTime = new DateTime(2018, 10, 10);
            _dateTimeGenerator
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
            _repository.ReplaceItemAsync(Arg.Is<TodoListItem>(arg => arg.Id == itemToEdit.Id)).Returns(expectedItem);
            _validator.Validate(Arg.Is<TodoListItem>(item => item.Id == expectedItem.Id))
                .Returns(ItemWithErrors.Create(expectedItem, new List<string>().AsReadOnly()));

            var resultItem = await _service.EditItemAsync(itemToEdit);

            Assert.That(resultItem.Item, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
            Assert.That(resultItem.WasOperationSuccessful, Is.True);
        }


        [Test]
        public async Task EditItemAsync_NonExistingItemSpecified_ReturnsNullItemWithErrors()
        {
            var updatedTime = new DateTime(2018, 10, 10);
            _dateTimeGenerator
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
            _repository.ReplaceItemAsync(itemToEdit).Returns((TodoListItem)null);
            _validator.Validate(Arg.Is<TodoListItem>(item => item.Id == expectedItem.Id))
                .Returns(ItemWithErrors.Create(expectedItem, new List<string>().AsReadOnly()));

            var resultItem = await _service.EditItemAsync(itemToEdit);

            Assert.That(resultItem.WasOperationSuccessful, Is.False);
            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                // The property Item must be called to throw an exception
                var callItem = resultItem.Item;
            });
        }
    }
}