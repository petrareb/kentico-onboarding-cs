using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.UrlLocation;
using MyOnboardingApp.Tests.Comparators;
using MyOnboardingApp.Tests.Utils;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private ITodoListRepository _repository; 
        private readonly Guid _expectedId = new Guid("00112233-4455-6677-8899-aabbccddeeff");
        private TodoListController _controller;
        private IUrlLocator _itemUrlLocator;

        [SetUp]
        public void SetUp()
        {
            _itemUrlLocator = Substitute.For<IUrlLocator>();
            _repository = Substitute.For<ITodoListRepository>();
            _controller = new TodoListController(_repository, _itemUrlLocator)
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
                new TodoListItem {Text = "1st Todo Item", Id = new Guid("11111111-1111-1111-1111-aabbccddeeff")},
                new TodoListItem {Text = "2nd Todo Item", Id = new Guid("22222222-2222-2222-2222-aabbccddeeff")}
            };
            _repository.GetAllItemsAsync().Returns(expectedItems);
            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync());
            message.TryGetContentValue(out TodoListItem[] itemsFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemsFromMessage, Is.SameAs(expectedItems));
        }


        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            var expectedItem = new TodoListItem { Text = "Default Item", Id = _expectedId };
            _repository.GetItemByIdAsync(_expectedId).Returns(expectedItem);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(_expectedId));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
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
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var itemToAdd = new TodoListItem {Text = "newText"};
            var expectedItem = new TodoListItem {Id = _expectedId, Text = "newText"};
            _repository.AddNewItemAsync(itemToAdd).Returns(expectedItem);
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(itemToAdd.Id).Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }
    }
}
