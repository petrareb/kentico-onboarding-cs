using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Models;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private readonly TodoListController _controller = new TodoListController();
        public TodoListItem TestItem { get; set; }

        [SetUp]
        public void SetUp()
        {
            TestItem = new TodoListItem{ Text = "coffee first" };
            TodoListController.Items.Add(TestItem);
        }

        [Test]
        public async Task Get_NoIdSpecified_ReturnsCorrectResponse()
        {
            var expectedItems  = TodoListController.Items;

            var response = await _controller.GetAsync();
            var msg = await response.ExecuteAsync(CancellationToken.None);
            msg.TryGetContentValue(out List<TodoListItem> items);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(expectedItems, Is.EqualTo(items));
        }

        [Test]
        public async Task Get_IdSpecified_ReturnsCorrectResponse()
        {
            var expectedItem = TodoListController.DefaultItem;

            var response = await _controller.GetAsync(expectedItem.Id);
            var msg = await response.ExecuteAsync(CancellationToken.None);
            msg.TryGetContentValue(out TodoListItem item);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(expectedItem, Is.EqualTo(item));

        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var result = await _controller.DeleteAsync(TestItem.Id);
            var statusCode = result.ExecuteAsync(CancellationToken.None).Result.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Delete_IdSpecified_RemovesItemWithId()
        {
            await _controller.DeleteAsync(TestItem.Id);

            Assert.That(TodoListController.Items, Has.No.Member(TestItem));
        }

        
        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsOkStatusCode()
        {
            var editedItem = TodoListController.Items.First();
            var newText = "aloha";
            editedItem.Text = newText;

            var result = await _controller.PutAsync(editedItem.Id, new TodoListItem{ Text = newText });
            var statusCode = result.ExecuteAsync(CancellationToken.None).Result.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async Task Put_IdSpecifiedTextSpecified_EditsItemWithId()
        {
            var editedItem = TodoListController.Items.First();
            var newText = "aloha";
            editedItem.Text = newText;

            await _controller.PutAsync(editedItem.Id, new TodoListItem{ Text = newText });

            Assert.That(TodoListController.Items, Has.Member(editedItem));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_ReturnsCorrectResponse()
        {
            var newText = "New One";

            var response = await _controller.PostAsync(new TodoListItem{ Text = newText });
            var msg = await response.ExecuteAsync(CancellationToken.None);
            msg.TryGetContentValue(out TodoListItem newItem);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(newItem.Text, Is.EqualTo(newText));
        }

        [Test]
        public async Task Post_NewTextSpecifiedInRequestBody_AddsNewItem()
        {
            var newText = "New One";
            await _controller.PostAsync(new TodoListItem{ Text = newText });
            Assert.That(TodoListController.Items.Any(item => item.Text == newText));
        }
    }
}
