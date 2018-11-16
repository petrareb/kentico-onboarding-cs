using System;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class DeleteItemServiceTests
    {
        private IDeleteItemService _deleteService;
        private readonly ITodoListRepository _repository = Substitute.For<ITodoListRepository>();


        [SetUp]
        public void SetUp()
            => _deleteService = new DeleteItemService(_repository);


        [Test]
        public async Task DeleteItemAsync_ExistingIdSpecified_ReturnsCorrectItemWithNoErrors()
        {
            var idToDelete = new Guid("00000000-0000-0000-0000-000000000001");
            var itemToDelete = new TodoListItem
            {
                Id = idToDelete,
                Text = "test",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            _repository.DeleteItemAsync(idToDelete).Returns(itemToDelete);

            var resultItem = await _deleteService.DeleteItemAsync(idToDelete);

            Assert.That(resultItem.Item, Is.EqualTo(itemToDelete).UsingItemEqualityComparer());
            Assert.That(resultItem.WasOperationSuccessful, Is.True);
        }


        [Test]
        public async Task DeleteItemAsync_NonExistingIdSpecified_ReturnsNullItemWithErrors()
        {
            var idToDelete = new Guid("00000000-0000-0000-0000-000000000002");
            _repository.DeleteItemAsync(idToDelete).Returns((TodoListItem)null);

            var resultItem = await _deleteService.DeleteItemAsync(idToDelete);
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