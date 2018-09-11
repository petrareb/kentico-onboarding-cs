using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Content.Models;
using MyOnboardingApp.Content.Repository;
using MyOnboardingApp.Tests.Utils;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private readonly ITodoListRepository _repository = Substitute.For<ITodoListRepository>();
        private readonly Guid _expectedId = Guid.Empty;


        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
        {
            var controller = new TodoListController(_repository);
            
            var msg = await controller.GetMessageFromAction(control => control.GetAsync());

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            _repository.GetItemByIdAsync(_expectedId)
                .Returns(new TodoListItem { Text = "Default Item", Id = _expectedId });
            var controller = new TodoListController(_repository);
            
            var msg = await controller.GetMessageFromAction(control => control.GetAsync(_expectedId));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            _repository.DeleteItemAsync(_expectedId)
                .Returns(new TodoListItem {Text = "Default Item", Id = _expectedId});
            var controller = new TodoListController(_repository);

            var msg = await controller.GetMessageFromAction(control => control.DeleteAsync(_expectedId));
            var statusCode = msg.StatusCode;
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var controller = new TodoListController(_repository);

            var msg = await controller.GetMessageFromAction(control => control.PutAsync(_expectedId, new TodoListItem { Text = "newText" }));
            var statusCode = msg.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var itemToAdd = new TodoListItem {Id = _expectedId, Text = "newText"};
            _repository.AddNewItemAsync(itemToAdd).Returns(new TodoListItem {Id = _expectedId, Text = "newText"});
            var controller = new TodoListController(_repository);

            var msg = await controller.GetMessageFromAction(control => control.PostAsync(itemToAdd));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }
    }
}
