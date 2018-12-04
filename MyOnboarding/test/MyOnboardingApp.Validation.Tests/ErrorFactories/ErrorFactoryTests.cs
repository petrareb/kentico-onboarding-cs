using System;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Validation.ErrorFactories;
using NUnit.Framework;

namespace MyOnboardingApp.Validation.Tests.ErrorFactories
{
    [TestFixture]
    public class ErrorFactoryTests
    {
        private readonly IErrorFactory _factory = new ErrorFactory();


        [Test]
        public void CreateValidationError_ValidPropertySelectorSpecified_ReturnsCorrectError()
        {
            const string errorDescription = "Validation Error happened.";
            const ErrorCode errorCode = ErrorCode.DataValidationError;
            var item = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "hippo",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2017, 05, 13)
            };
            var expectedResult = new Error(errorCode, errorDescription, nameof(item.Text));

            var result = _factory.CreateValidationError(() => item.Text, errorDescription);

            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Location, Is.EqualTo(expectedResult.Location));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }


        [Test]
        public void CreateValidationError_ValidPropertySelectorSpecified_ReturnsCorrectError2()
        {
            const string errorDescription = "Validation Error happened.";
            const ErrorCode errorCode = ErrorCode.DataValidationError;
            var item = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "hippo",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2017, 05, 13)
            };
            var expectedResult = new Error(errorCode, errorDescription, nameof(item.LastUpdateTime));

            var result = _factory.CreateValidationError(() => item.LastUpdateTime, errorDescription);

            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Location, Is.EqualTo(expectedResult.Location));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }


        [Test]
        public void CreateValidationError_InvalidPropertyInfoOfSelectorSpecified_ThrowsException()
        {
            const string errorDescription = "Validation Error happened.";
            var item = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = "po",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2018, 05, 13)
            };


            Assert.Throws<InvalidCastException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var result = _factory.CreateValidationError(() => item.Text.Equals(string.Empty), errorDescription);
            });
        }


        [Test]
        public void CreateValidationError_InvalidMemberExpressionOfPropertySelectorSpecified_ThrowsException()
        {
            const string errorDescription = "Validation Error happened.";


            Assert.Throws<InvalidCastException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var result = _factory.CreateValidationError(() => 4 == 10, errorDescription);
            });
        }


        [Test]
        public void CreatePermissionError_ValidPropertySelectorSpecified_ReturnsCorrectError()
        {
            const string errorDescription = "Permission Error happened.";
            const ErrorCode errorCode = ErrorCode.PermissionError;
            var item = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Text = "mus",
                CreationTime = new DateTime(2016, 01, 01),
                LastUpdateTime = new DateTime(2017, 05, 13)
            };
            var expectedResult = new Error(errorCode, errorDescription, nameof(item.Text));

            var result = _factory.CreatePermissionError(() => item.Text, errorDescription);

            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Location, Is.EqualTo(expectedResult.Location));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}