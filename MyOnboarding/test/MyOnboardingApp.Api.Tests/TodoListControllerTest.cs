using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Content.Models;
using MyOnboardingApp.Content.Repository;
using MyOnboardingApp.Contracts.UrlLocation;
using MyOnboardingApp.Tests.Utils;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private ITodoListRepository _repository; 
        private readonly Guid _expectedId = Guid.Empty;
        private TodoListController _controller;
        private IUrlLocator _itemUrlLocator;

        [SetUp]
        public async Task SetUp()
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
            var msg = await _controller.GetMessageFromAction(control => control.GetAsync());

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            _repository.GetItemByIdAsync(_expectedId)
                .Returns(new TodoListItem { Text = "Default Item", Id = _expectedId });

            var msg = await _controller.GetMessageFromAction(control => control.GetAsync(_expectedId));
            msg.TryGetContentValue(out TodoListItem itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            _repository.DeleteItemAsync(_expectedId)
                .Returns(new TodoListItem { Text = "Default Item", Id = _expectedId });

            var msg = await _controller.GetMessageFromAction(control => control.DeleteAsync(_expectedId));
            var statusCode = msg.StatusCode;
            msg.TryGetContentValue(out TodoListItem itemFromMsg);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var msg = await _controller.GetMessageFromAction(control => control.PutAsync(_expectedId, new TodoListItem { Text = "newText" }));
            var statusCode = msg.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var itemToAdd = new TodoListItem { Id = _expectedId, Text = "newText" };
            _repository.AddNewItemAsync(itemToAdd).Returns(new TodoListItem { Id = _expectedId, Text = "newText" });

            var msg = await _controller.GetMessageFromAction(control => control.PostAsync(itemToAdd));
            msg.TryGetContentValue(out TodoListItem itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }
    }
}
