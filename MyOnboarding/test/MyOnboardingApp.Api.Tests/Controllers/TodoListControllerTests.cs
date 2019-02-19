using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Tests.Extensions;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Urls;
using NSubstitute;
using NUnit.Framework;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.Contracts.Services;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Factories;

namespace MyOnboardingApp.Api.Tests.Controllers
{
    [TestFixture]
    public class TodoListControllerTests
    {
        private TodoListController _controller;
        private IUrlLocator _itemUrlLocator;
        private IRetrieveItemService _retrieveService;
        private ICreateItemService _addNewService;
        private IDeleteItemService _deleteService;
        private IUpdateItemService _editService;
        private ITodoListRepository _repository;


        [SetUp]
        public void SetUp()
        {
            _itemUrlLocator = Substitute.For<IUrlLocator>();
            _retrieveService = Substitute.For<IRetrieveItemService>();
            _addNewService = Substitute.For<ICreateItemService>();
            _deleteService = Substitute.For<IDeleteItemService>();
            _editService = Substitute.For<IUpdateItemService>();
            _repository = Substitute.For<ITodoListRepository>();


            _controller = new TodoListController(_itemUrlLocator, _repository, _retrieveService, _addNewService, _deleteService, _editService)
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
                new TodoListItem
                {
                    Text = "1st Todo Item",
                    Id = new Guid("11111111-1111-1111-1111-aabbccddeeff"),
                    CreationTime = new DateTime(1995, 01, 01),
                    LastUpdateTime = new DateTime(1996, 01, 01)
                },
                new TodoListItem
                {
                    Text = "2nd Todo Item",
                    Id = new Guid("22222222-2222-2222-2222-aabbccddeeff"),
                    CreationTime = new DateTime(2000, 01, 01),
                    LastUpdateTime = new DateTime(2001, 01, 01)
                }
            };
            _repository
                .GetAllItemsAsync()
                .Returns(expectedItems);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync());
            message.TryGetContentValue(out IEnumerable<TodoListItem> itemsFromMessage);

            await _repository
                .Received(1)
                .GetAllItemsAsync();
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemsFromMessage, Is.SameAs(expectedItems));
        }


        [Test]
        public async Task Get_IdSpecified_ReturnsOkStatusCodeAndExpectedItemWithNoErrors()
        {
            var expectedId = new Guid("00000000-0000-0000-0001-aabbccddeeff");
            var expectedItem = new TodoListItem
            {
                Text = "Test Item",
                Id = expectedId
            };
            var expectedItemWithStatus = ItemVariantsFactory.CreateItemVariants(expectedItem).ResolvedItem;
            _retrieveService
                .GetItemByIdAsync(expectedId)
                .Returns(expectedItemWithStatus);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(expectedId));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            await _retrieveService
                .Received(1)
                .GetItemByIdAsync(expectedId);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Get_ItemWithEmptyIdSpecified_ReturnsBadRequestStatusCode()
        {
            var id = Guid.Empty;

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(id));

            await _retrieveService
                .Received(0)
                .GetItemByIdAsync(id);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Get_NonExistingIdSpecified_ReturnsNotFoundStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0002-aabbccddeeff");
            var expectedItemWithStatus = ResolvedItem.Create((TodoListItem)null);
            _retrieveService.GetItemByIdAsync(expectedId).Returns(expectedItemWithStatus);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.GetAsync(expectedId));

            await _retrieveService
                .Received(1)
                .GetItemByIdAsync(expectedId);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


        [Test]
        public async Task Delete_IdSpecified_ReturnsOkStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0003-aabbccddeeff");
            var expectedItem = new TodoListItem { Text = "Test Item", Id = expectedId };
            var expectedItemWithStatus = ItemVariantsFactory.CreateItemVariants(expectedItem).ResolvedItem;
            _deleteService
                .DeleteItemAsync(expectedId)
                .Returns(expectedItemWithStatus);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.DeleteAsync(expectedId));
            var statusCode = message.StatusCode;
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            await _deleteService
                .Received(1)
                .DeleteItemAsync(expectedId);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Delete_NonExistingIdSpecified_ReturnsNotFoundStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0004-aabbccddeeff");
            _deleteService.DeleteItemAsync(expectedId).Returns(ResolvedItem.Create((TodoListItem)null));

            var message = await _controller.GetMessageFromActionAsync(controller => controller.DeleteAsync(expectedId));
            var statusCode = message.StatusCode;

            await _deleteService
                .Received(1)
                .DeleteItemAsync(expectedId);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


        [Test]
        public async Task Delete_IdNotSpecified_ReturnsBadRequestStatusCode()
        {
            var id = Guid.Empty;

            var message = await _controller.GetMessageFromActionAsync(controller => controller.DeleteAsync(id));
            var statusCode = message.StatusCode;

            await _deleteService
                .Received(0)
                .DeleteItemAsync(id);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Put_IdSpecifiedTextSpecified_ReturnsNoContentStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0005-aabbccddeeff");
            var (_, existingResolvedItem, _) = ItemVariantsFactory.CreateItemVariants(
                id: expectedId,
                text: "hello",
                creationTime: new DateTime(2000, 01, 01));
            var itemToPut = new TodoListItem
            {
                Id = expectedId,
                Text = "text"
            };
            var expectedResponse = ItemVariantsFactory.CreateItemVariants(itemToPut, new List<Error>()).ItemWithErrors;
            _retrieveService
                .GetItemByIdAsync(expectedId)
                .Returns(existingResolvedItem);
            _editService
                .EditItemAsync(itemToPut, existingResolvedItem)
                .Returns(expectedResponse);
            
            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(expectedId, itemToPut));
            var statusCode = message.StatusCode;

            await _editService
                .Received(1)
                .EditItemAsync(itemToPut, existingResolvedItem);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }


        [Test]
        public async Task Put_DifferentIdSpecifiedInUrlAndParameters_ReturnsBadRequestStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0006-aabbccddeeff");
            var itemId = new Guid("00000000-0000-0000-0000-000000000007");
            var itemToPut = ItemVariantsFactory.CreateItemVariants(
                id: itemId,
                text: "text"
            ).Item;

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(expectedId, itemToPut));
            var statusCode = message.StatusCode;

            await _editService
                .Received(0)
                .EditItemAsync(itemToPut, Arg.Any<IResolvedItem<TodoListItem>>());
            await _retrieveService
                .Received(0)
                .GetItemByIdAsync(expectedId);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Put_ItemWithEmptyTextSpecified_ReturnsNoContentStatusCode()
        {
            var expectedId = new Guid("00000000-0000-0000-0005-aabbccddeeff");
            var (existingItem, existingResolvedItem, _) = ItemVariantsFactory.CreateItemVariants(
                id: expectedId,
                text: "text",
                creationTime: new DateTime(2000, 01, 01));
            var itemToPut = ItemVariantsFactory.CreateItemVariants(
                id: expectedId,
                text: string.Empty
            ).Item;
            var error = new Error(ErrorCode.DataValidationError, "empty text", "Text");
            var expectedResponse = ItemVariantsFactory.CreateItemVariants(itemToPut, new List<Error> { error }).ItemWithErrors;
            _editService
                .EditItemAsync(itemToPut, existingResolvedItem)
                .Returns(expectedResponse);
            _retrieveService
                .GetItemByIdAsync(expectedId)
                .Returns(ResolvedItem.Create(existingItem));

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(expectedId, itemToPut));
            var statusCode = message.StatusCode;

            await _editService
                .Received(0)
                .EditItemAsync(Arg.Is<TodoListItem>(item => item.Id == expectedId), existingResolvedItem);
            await _retrieveService
                .Received(0)
                .GetItemByIdAsync(expectedId);
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Put_NonExistingItem_CreatesNewItem()
        {
            var itemId = new Guid("00000000-0000-0000-0007-bbbbbbbbbbbb");
            const string itemText = "text";
            var itemToPut = ItemVariantsFactory.CreateItemVariants(
                id: itemId,
                text: itemText
            ).Item;
            var expectedItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0007-aaaaaaaaaaaa"),
                text: itemText,
                creationTime: new DateTime(2009, 09, 09)
            ).Item;
            var (_, expectedResponseResolved, expectedResponseWithErrors) = ItemVariantsFactory.CreateItemVariants(expectedItem);

            _retrieveService
                .GetItemByIdAsync(itemId)
                .Returns(ResolvedItem.Create<TodoListItem>(null));
            _addNewService
                .AddNewItemAsync(Arg.Is<TodoListItem>(item => item.Text == itemText))
                .Returns(expectedResponseWithErrors);


            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(itemId, itemToPut));
            var statusCode = message.StatusCode;
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            await _editService
                .Received(0)
                .EditItemAsync(itemToPut, expectedResponseResolved);
            await _retrieveService
                .Received(1)
                .GetItemByIdAsync(itemId);
            await _addNewService
                .Received(1).
                AddNewItemAsync(Arg.Is<TodoListItem>(item => item.Text == itemText));
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Put_NullItemInRequestBodySpecified_ReturnsBadRequest()
        {
            var testItem = (TodoListItem)null;
            var urlId = new Guid("00000000-0000-0000-0008-aabbccddeeff");

            // ReSharper disable once ExpressionIsAlwaysNull
            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(urlId, testItem));

            await _editService
                .Received(0)
                .EditItemAsync(Arg.Any<TodoListItem>(), Arg.Any<IResolvedItem<TodoListItem>>());
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Put_ItemWithEmptyIdSpecified_ReturnsBadRequest()
        {
            var urlId = new Guid("00000000-0000-0000-0009-aabbccddeeff");
            var testItem = new TodoListItem
            {
                Text = "newText",
                Id = Guid.Empty,
                CreationTime = new DateTime(2000, 01, 01),
                LastUpdateTime = new DateTime(2000, 01, 01)
            };

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(urlId, testItem));

            await _editService
                .Received(0)
                .EditItemAsync(Arg.Any<TodoListItem>(), Arg.Any<IResolvedItem<TodoListItem>>());
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Put_InvalidItemReturnedToController_ReturnsBadRequest()
        {
            var expectedId = new Guid("00000000-0000-0000-0005-aabbccddeeff");
            var (_, existingResolvedItem, _) = ItemVariantsFactory.CreateItemVariants(
                id: expectedId,
                text: "hello",
                creationTime: new DateTime(2000, 01, 01));
            var itemToPut = new TodoListItem
            {
                Id = expectedId,
                Text = "text"
            };
            _editService.EditItemAsync(Arg.Any<TodoListItem>(), Arg.Any<IResolvedItem<TodoListItem>>()).Returns(
                ItemWithErrors.Create(itemToPut, new[] {new Error(ErrorCode.DataValidationError, "error message", "error.location")}));
            _retrieveService.GetItemByIdAsync(expectedId).Returns(existingResolvedItem);


            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(expectedId, itemToPut));

            await _editService
                .Received(1)
                .EditItemAsync(itemToPut, existingResolvedItem);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_CorrectAttributesInRequestBody_ReturnsCorrectResponseAndItemWithNoErrors()
        {
            var expectedId = new Guid("00000000-0000-0000-0012-aabbccddeeff");
            var itemToAdd = new TodoListItem
            {
                Text = "newText",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var updatedTime = new DateTime(2018, 10, 10);
            var expectedItem = new TodoListItem
            {
                Id = expectedId,
                Text = "newText",
                CreationTime = updatedTime,
                LastUpdateTime = updatedTime
            };
            var expectedItemWithStatus = ItemVariantsFactory.CreateItemVariants(expectedItem).ItemWithErrors;
            _addNewService
                .AddNewItemAsync(itemToAdd)
                .Returns(expectedItemWithStatus);
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));
            message.TryGetContentValue(out TodoListItem itemFromMessage);

            await _addNewService
                .Received(1)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(itemFromMessage, Is.EqualTo(expectedItem).UsingItemEqualityComparer());
        }


        [Test]
        public async Task Post_EmptyTextSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            await _addNewService
                .Received(0)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_InvalidCreationTimeSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = new DateTime(2018, 10, 10),
                LastUpdateTime = DateTime.MinValue
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            await _addNewService
                .Received(0)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_InvalidItemReturnedToController_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var invalidItem = ItemWithErrors.Create(itemToAdd, new List<Error>(new[] { new Error(ErrorCode.DataValidationError, "error", "error.location") }));
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);
            _addNewService.AddNewItemAsync(itemToAdd).Returns(invalidItem);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            await _addNewService
                .Received(1)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_InvalidLastUpdateTimeSpecifiedInRequestBody_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = new DateTime(2018, 10, 10)
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            await _addNewService
                .Received(0)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_NullItemSpecified_ReturnsBadRequest()
        {
            var testItem = (TodoListItem)null;
            var urlId = new Guid("00000000-0000-0000-0008-aabbccddeeff");
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(urlId)
                .Returns(expectedUrl);

            // ReSharper disable once ExpressionIsAlwaysNull
            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(testItem));

            await _addNewService
                .Received(0)
                // ReSharper disable once ExpressionIsAlwaysNull
                .AddNewItemAsync(testItem);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task Post_ItemWithNonEmptyIdSpecified_ReturnsBadRequest()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = new Guid("00000000-0000-0000-0009-aabbccddeeff"),
            };
            const string expectedUrl = "expected/Url";
            _itemUrlLocator
                .GetListItemUrl(itemToAdd.Id)
                .Returns(expectedUrl);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));

            await _addNewService
                .Received(0)
                .AddNewItemAsync(itemToAdd);
            Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
