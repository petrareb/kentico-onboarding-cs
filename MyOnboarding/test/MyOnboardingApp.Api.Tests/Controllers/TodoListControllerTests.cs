using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Tests.Extensions;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Urls;
using NSubstitute;
using NUnit.Framework;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.Contracts.Services;

namespace MyOnboardingApp.Api.Tests.Controllers
{
    [TestFixture]
    public class TodoListControllerTests
    {
        private ITodoListRepository _repository; 
        private readonly Guid _expectedId = new Guid("00112233-4455-6677-8899-aabbccddeeff");
        private TodoListController _controller;
        private IUrlLocator _itemUrlLocator;
        private IRetrieveItemService _retrieveService;
        private ICreateItemService _addNewService;


        [SetUp]
        public void SetUp()
        {
            _itemUrlLocator = Substitute.For<IUrlLocator>();
            _repository = Substitute.For<ITodoListRepository>();
            _retrieveService = Substitute.For<IRetrieveItemService>();
            _addNewService = Substitute.For<ICreateItemService>();

            _controller = new TodoListController(_repository, _itemUrlLocator, _retrieveService, _addNewService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Url = new UrlHelper()
            };
        }


        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
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
            _retrieveService.GetAllItemsAsync().Returns(expectedItems);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync());
            message.TryGetContentValue(out TodoListItem[] itemsFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemsFromMessage, Is.SameAs(expectedItems));
        }


        [Test]
        public async Task Get_IdSpecified_ReturnsOkStatusCodeAndExpectedItemWithNoErrors()
        {
            var expectedItem = new TodoListItem
            {
                Text = "Test Item",
                Id = _expectedId
            };
            var expectedItemWithStatus = ResolvedItem.Create(expectedItem);
            _retrieveService.GetItemByIdAsync(_expectedId).Returns(expectedItemWithStatus);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(_expectedId));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Get_EmptyIdSpecified_ReturnsBadRequestStatusCode()
        {
            var id = Guid.Empty;

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(id));

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Get_NonExistingIdSpecified_ReturnsNotFoundStatusCode()
        {
            var expectedItemWithStatus = ResolvedItem.Create((TodoListItem)null);
            _retrieveService.GetItemByIdAsync(_expectedId).Returns(expectedItemWithStatus);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(_expectedId));

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var expectedItem = new TodoListItem {Text = "Default Item", Id = _expectedId};
            _repository.DeleteItemAsync(_expectedId).Returns(expectedItem);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.DeleteAsync(_expectedId));
            var statusCode = message.StatusCode;
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsCorrectStatusCode()
        {
            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(_expectedId, new TodoListItem { Text = "newText" }));
            var statusCode = message.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }


        [Test]
        public async Task Post_CorrectAttributesInRequestBody_ReturnsCorrectResponseAndItemWithNoErrors()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "newText",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var updatedTime = new DateTime(2018, 10, 10);
            var expectedItem = new TodoListItem
            {
                Id = _expectedId,
                Text = "newText",
                CreationTime = updatedTime,
                LastUpdateTime = updatedTime
            };
            var expectedItemWithStatus = ResolvedItem.Create(expectedItem);
            _addNewService.AddNewItemAsync(itemToAdd).Returns(expectedItemWithStatus);
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(itemToAdd.Id).Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Post_EmptyTextSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(itemToAdd.Id).Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_InvalidCreationTimeSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = new DateTime(2018, 10, 10),
                LastUpdateTime = DateTime.MinValue
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(itemToAdd.Id).Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_InvalidLastUpdateTimeSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = new DateTime(2018, 10, 10)
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(itemToAdd.Id).Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
