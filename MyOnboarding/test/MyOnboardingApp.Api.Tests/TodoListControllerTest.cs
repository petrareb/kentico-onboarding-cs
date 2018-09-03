using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Entities;
using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class TodoListControllerTest
    {
        private TodoListController controller = new TodoListController();
        public TodoListItem TestItem = new TodoListItem("coffee first");

        [SetUp]
        public void SetUp()
        {
            controller.Items.Add(TestItem);
        }

        [Test]
        public void GetItemById()
        {
            var expected = TestItem;
            var result = controller.Get(TestItem.Id);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DeleteItemById()
        {
            controller.Delete(TestItem.Id);

            Assert.That(controller.Items, Has.No.Member(TestItem));
        }

        [Test]
        public void GetAllItems()
        {
            var expected = controller.Items;
            var result = controller.Get();

            Assert.That(result, Is.EqualTo(expected));
        }

        //[Test]
        //public void EditItemById()
        //{
        //    TestItem.Text = "aloha";

        //}

        //[Test]
        //public void AddNewItem()
        //{


        //}
    }

}
