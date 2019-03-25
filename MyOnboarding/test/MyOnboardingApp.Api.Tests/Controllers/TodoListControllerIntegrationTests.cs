using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MyOnboardingApp.Api.Controllers;
using MyOnboardingApp.Api.Tests.Extensions;
using MyOnboardingApp.Contracts.Dependencies;
using MyOnboardingApp.Contracts.Generators;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Registration;
using MyOnboardingApp.Contracts.Repository;
using MyOnboardingApp.Contracts.Urls;
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace MyOnboardingApp.Api.Tests.Controllers
{
    [TestFixture]
    public class TodoListControllerIntegrationTests
    {
        private TodoListController _controller;
        private ITodoListRepository _repository;
        private IIdGenerator<Guid> _idGenerator;
        private IDateTimeGenerator _dateTimeGenerator;


        [SetUp]
        public void SetUp()
        {
            _idGenerator = Substitute.For<IIdGenerator<Guid>>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();
            _repository = Substitute.For<ITodoListRepository>();

            var itemUrlLocator = Substitute.For<IUrlLocator>();

            var unityContainer = new UnityContainer();
            Bootstrap
                .Create(unityContainer, new List<IBootstrapper>())
                .RegisterAllDependencies();

            unityContainer.RegisterInstance(typeof(IIdGenerator<Guid>), _idGenerator);
            unityContainer.RegisterInstance(typeof(IDateTimeGenerator), _dateTimeGenerator);
            unityContainer.RegisterInstance(typeof(ITodoListRepository), _repository);
            unityContainer.RegisterInstance(typeof(IUrlLocator), itemUrlLocator);

            _controller = unityContainer.Resolve<TodoListController>();
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();
        }


        [Test]
        public async Task Post_ValidItemWithMinDateTimesSpecified_ReturnsCreatedStatusCode()
        {
            var itemToAdd = new TodoListItem
            {
                Text = "hello",
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = DateTime.MinValue
            };
            var newId = new Guid("00000000-0000-0000-0001-aabbccddeeff");
            var newDateTime = new DateTime(2019, 02, 07);
            _idGenerator.GetNewId().Returns(newId);
            _dateTimeGenerator.GetCurrentDateTime().Returns(newDateTime);

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PostAsync(itemToAdd));
            var statusCode = message.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.Created));
        }


        [Test]
        public async Task Put_ValidItemReturnedFromRepository_ReturnsNoContentStatusCode()
        {
            var existingId = new Guid("00000000-0000-0000-0002-aabbccddeeff");
            var existingItem = new TodoListItem
            {
                Text = "hello",
                Id = existingId,
                CreationTime = new DateTime(2018, 10, 10),
                LastUpdateTime = new DateTime(2018, 10, 10)
            };
            _repository.GetItemByIdAsync(existingId).Returns(existingItem);
            var currentDateTime = new DateTime(2019, 02, 07);
            _dateTimeGenerator.GetCurrentDateTime().Returns(currentDateTime);

            var itemToPut = new TodoListItem
            {
                Text = "new text",
                Id = existingId,
            };

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(existingId, itemToPut));
            var statusCode = message.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }


        [Test]
        public async Task Put_InvalidItemWithCreationMinDateTimeReturnedFromRepository_ReturnsBasRequestStatusCode()
        {
            var existingId = new Guid("00000000-0000-0000-0003-aabbccddeeff");
            var existingItem = new TodoListItem
            {
                Text = "hello",
                Id = existingId,
                CreationTime = DateTime.MinValue,
                LastUpdateTime = new DateTime(2018, 10, 10)
            };
            _repository.GetItemByIdAsync(existingId).Returns(existingItem);
            var currentDateTime = new DateTime(2019, 02, 07);
            _dateTimeGenerator.GetCurrentDateTime().Returns(currentDateTime);

            var itemToPut = new TodoListItem
            {
                Text = "new text",
                Id = existingId,
            };

            var message = await _controller.GetMessageFromActionAsync(controller => controller.PutAsync(existingId, itemToPut));
            var statusCode = message.StatusCode;

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}