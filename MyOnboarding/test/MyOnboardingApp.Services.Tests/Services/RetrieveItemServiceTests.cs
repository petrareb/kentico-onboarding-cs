using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.TestUtils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Services
{
    [TestFixture]
    public class RetrieveItemServiceTests
    {
        private IRetrieveItemService _service;
        private readonly ITodoListRepository _repository = Substitute.For<ITodoListRepository>();


        [SetUp]
        public void SetUp()
            => _service = new RetrieveItemService(_repository);


        [Test]
        public async Task Get_NoIdSpecified_ReturnsAllItems()
        {
            var expectedItems = new[]
            {
                new TodoListItem
                {
                    Text = "1st Todo Item",
                    Id = new Guid("11111111-1111-1111-1111-aabbccddeeff"),
                    CreationTime = new DateTime(1995, 01, 01),
                    LastUpdateTime = new DateTime(1996, 01, 01)
                },
                new TodoListItem
                {
                    Text = "2nd Todo Item",
                    Id = new Guid("22222222-2222-2222-2222-aabbccddeeff"),
                    CreationTime = new DateTime(2000, 01, 01),
                    LastUpdateTime = new DateTime(2001, 01, 01)
                }
            };
            _repository.GetAllItemsAsync().Returns(expectedItems);

            var result = await _service.GetAllItemsAsync();

            Assert.That(result, Is.SameAs(expectedItems));
        }


        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectItemWithNoErrors()
        {
            var requestedId = new Guid("00000000-0000-0000-0000-000000000007");
            var expectedItem = new TodoListItem
            {
                Text = "Test Item",
                Id = requestedId
            };
            _repository.GetItemByIdAsync(requestedId).Returns(expectedItem);

            var result = await _service.GetItemByIdAsync(requestedId);

            Assert.That(result.Item, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
            Assert.That(result.WasOperationSuccessful, Is.True);
        }


        [Test]
        public async Task Get_NonExistingIdSpecified_ReturnsItemWithErrors()
        {
            var requestedId = new Guid("00000000-0000-0000-0000-000000000007");
            _repository.GetItemByIdAsync(requestedId).Returns((TodoListItem)null);

            var result = await _service.GetItemByIdAsync(requestedId);

            Assert.Throws<InvalidOperationException>(() =>
            {
                // ReSharper disable once UnusedVariable
                // The property Item must be called to throw an exception
                var callItem = result.Item;
            });
            Assert.That(result.WasOperationSuccessful, Is.False);
        }
    }
}