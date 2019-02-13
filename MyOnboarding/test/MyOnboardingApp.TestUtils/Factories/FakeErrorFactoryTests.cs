using System;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.TestUtils.Extensions;
using NUnit.Framework;

namespace MyOnboardingApp.TestUtils.Factories
{
    [TestFixture]
    public class FakeErrorFactoryTests
    {
        [Test]
        public void CreateValidationError_ValidPropertySelectorAndMessageSpecified_ReturnsCorrectError()
        {
            var factory = new FakeErrorFactory();
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = string.Empty,
                CreationTime = new DateTime(2000, 01, 01),
                LastUpdateTime = new DateTime(2000, 01, 01)
            };
            const string errorDescription = "validation error";
            var expectedError = new Error(ErrorCode.DataValidationError, errorDescription, nameof(testItem.Text));

            var result = factory.CreateValidationError(() => testItem.Text, errorDescription);

            Assert.That(result, Is.EqualTo(expectedError).UsingErrorEqualityComparer());
        }
    }
}