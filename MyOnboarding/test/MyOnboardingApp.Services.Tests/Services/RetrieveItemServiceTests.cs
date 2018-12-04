using System;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Services.Services;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
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
        public async Task Get_IdSpecified_ReturnsCorrectItemWithNoErrors()
        {
            var requestedId = new Guid("00000000-0000-0000-0000-000000000007");
            var expectedItem = new TodoListItem
            {
                Text = "Test Item",
                Id = requestedId,
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };
            var expectedResult = ItemVariantsFactory.CreateItemVariants(expectedItem).ResolvedItem;
            _repository.GetItemByIdAsync(requestedId).Returns(expectedItem);

            var result = await _service.GetItemByIdAsync(requestedId);

            await _repository.Received(1).GetItemByIdAsync(requestedId);
            Assert.That(result, Is.EqualTo(expectedResult).UsingResolvedItemEqualityComparer());
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