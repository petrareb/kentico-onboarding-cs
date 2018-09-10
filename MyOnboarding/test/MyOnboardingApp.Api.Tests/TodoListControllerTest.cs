using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Models;
using MyOnboardingApp.Tests.Utils;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private readonly TodoListController _controller = new TodoListController();

        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.GetAsync());

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            var expectedItem = TodoListController.S_defaultItem;

            var msg = await _controller.GetMessageFromAction(controller => controller.GetAsync(expectedItem.Id));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(expectedItem.Id, Is.EqualTo(itemFromMsg.Id));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var idToDelete = TodoListController.S_defaultItem.Id;

            var msg = await _controller.GetMessageFromAction(controller => controller.DeleteAsync(idToDelete));
            var statusCode = msg.StatusCode;
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(idToDelete));
        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var editedItem = TodoListController.S_defaultItem;

            var msg = await _controller.GetMessageFromAction(controller => controller.PutAsync(editedItem.Id, new TodoListItem { Text = "newText" }));
            var statusCode = msg.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var newId = Guid.NewGuid();

            var msg = await _controller.GetMessageFromAction(controller => controller.PostAsync(new TodoListItem { Id = newId, Text = "newText" }));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMsg.Id, Is.EqualTo(newId));
        }
    }
}
