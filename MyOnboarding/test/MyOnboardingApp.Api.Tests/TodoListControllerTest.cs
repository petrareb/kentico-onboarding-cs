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
        //public TodoListItem TestItem { get; set; }

        //[SetUp]
        //public void SetUp()
        //{
        //    TestItem = new TodoListItem{ Text = "coffee first" };
        //    //TodoListController.Items.Add(TestItem);
        //}

        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
        {
            var response = await _controller.GetResponseFromAction(controller => controller.GetAsync());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            var expectedItem = TodoListController.S_defaultItem;
            var response = await _controller.GetResponseFromAction(controller => controller.GetAsync(expectedItem.Id));
            TodoListItem itemFromResponse;
            response.TryGetContentValue(out itemFromResponse);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(expectedItem.Id, Is.EqualTo(itemFromResponse.Id));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var idToDelete = TodoListController.S_defaultItem.Id;
            var response = await _controller.GetResponseFromAction(controller => controller.DeleteAsync(idToDelete));
            var statusCode = response.StatusCode;
            TodoListItem itemFromResponse;

            response.TryGetContentValue(out itemFromResponse);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromResponse.Id, Is.EqualTo(idToDelete));
        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var editedItem = TodoListController.S_defaultItem;

            var response = await _controller.GetResponseFromAction(controller => controller.PutAsync(editedItem.Id, new TodoListItem { Text = "newText" }));

            var statusCode = response.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var newId = Guid.NewGuid();

            var response = await _controller.GetResponseFromAction(controller => controller.PostAsync(new TodoListItem { Id = newId, Text = "newText" }));

            TodoListItem responseItem;
            response.TryGetContentValue(out responseItem);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(responseItem.Id, Is.EqualTo(newId));
        }
    }
}
