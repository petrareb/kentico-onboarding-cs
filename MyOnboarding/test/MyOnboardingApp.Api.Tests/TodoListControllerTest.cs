using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            TestItem = new TodoListItem("coffee first");
            TodoListController.Items.Add(TestItem);
        }

        [Test]
        public void GetItemById()
        {
            // returns predefined value
            var expected = TodoListController.DefaultItem;
            var result = _controller.Get(TestItem.Id);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DeleteItemById()
        {
            _controller.Delete(TestItem.Id);

            Assert.That(TodoListController.Items, Has.No.Member(TestItem));
        }

        [Test]
        public void GetAllItems()
        {
            var expected = TodoListController.Items;
            var result = _controller.Get();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EditItemById()
        {
            var editedItem = TodoListController.Items.First();
            var newText = "aloha";
            editedItem.Text = newText;

            _controller.Put(editedItem.Id, new TodoListItem(newText));

            Assert.That(TodoListController.Items, Has.Member(editedItem));
        }

        [Test]
        public void AddNewItem()
        {
            var newText = "New One";
            _controller.Post(new TodoListItem(newText));

            Assert.That(TodoListController.Items.Any(item => item.Text == newText));
        }
    }
}
