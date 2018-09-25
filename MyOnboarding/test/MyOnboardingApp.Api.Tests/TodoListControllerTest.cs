using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
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
        private readonly Guid _expectedId = new Guid("00112233-4455-6677-8899-aabbccddeeff");
        private TodoListController _controller;
        private IUrlLocator _itemUrlLocator;
        private IEqualityComparer<TodoListItem> _comparer;

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
            _comparer = new ItemEqualityComparer();
            await Task.CompletedTask;
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
            var expectedItem = new TodoListItem { Text = "Default Item", Id = _expectedId };
            _repository.GetItemByIdAsync(_expectedId)
                .Returns(expectedItem);

            var msg = await _controller.GetMessageFromAction(control => control.GetAsync(_expectedId));
            msg.TryGetContentValue(out TodoListItem itemFromMsg);

            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg, Is.EqualTo(expectedItem).Using(_comparer));
        }

        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var expectedItem = new TodoListItem {Text = "Default Item", Id = _expectedId};
            _repository.DeleteItemAsync(_expectedId)
                .Returns(expectedItem);

            var msg = await _controller.GetMessageFromAction(control => control.DeleteAsync(_expectedId));
            var statusCode = msg.StatusCode;
            msg.TryGetContentValue(out TodoListItem itemFromMsg);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMsg, Is.EqualTo(expectedItem).Using(_comparer));
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
            var itemToAdd = new TodoListItem {Text = "newText"};
            var expectedItem = new TodoListItem {Id = _expectedId, Text = "newText"};
            _repository.AddNewItemAsync(itemToAdd).Returns(expectedItem);
            const string expectedUrl = "expected/Url";
            _itemUrlLocator.GetListItemUrl(Arg.Any<Guid>()).Returns(expectedUrl);

            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "ListItemUrl",
                routeTemplate: "api/v{version}/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary{{"version", "1.0"}, {"controller", "todolist"}});
        

            var msg = await _controller.GetMessageFromAction(control => control.PostAsync(itemToAdd));
            msg.TryGetContentValue(out TodoListItem itemFromMsg);
            Assert.That(msg.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMsg, Is.EqualTo(expectedItem).Using(_comparer));
        }
    }
}
