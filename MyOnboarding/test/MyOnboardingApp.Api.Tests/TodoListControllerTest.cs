using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Content.Models;
using MyOnboardingApp.Tests.Utils;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private readonly TodoListController _controller = new TodoListController();
        private readonly Guid _expectedId = Guid.Empty;

        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.GetAsync());

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.GetAsync(_expectedId));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.DeleteAsync(_expectedId));
            var statusCode = msg.StatusCode;
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.PutAsync(_expectedId, new TodoListItem { Text = "newText" }));
            var statusCode = msg.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var msg = await _controller.GetMessageFromAction(controller => controller.PostAsync(new TodoListItem { Id = _expectedId, Text = "newText" }));
            TodoListItem itemFromMsg;
            msg.TryGetContentValue(out itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMsg.Id, Is.EqualTo(_expectedId));
        }
    }
}
