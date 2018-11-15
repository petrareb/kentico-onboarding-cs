using System;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Services.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Validation
{
    [TestFixture]
    public class TextCheckingValidatorTests
    {
        private readonly TextCheckingValidator _validator = new TextCheckingValidator();


        [Test]
        public void Validate_ItemWithWhiteSpaceTextSpecified_ReturnsFalse()
        {
            var itemToTest = new TodoListItem
            {
                Id = new Guid("00112233-4455-6677-8899-aabbccddeeff"),
                Text = "          ",
                CreationTime = new DateTime(2018, 01, 01),
                LastUpdateTime = new DateTime(2018, 01, 01)
            };

            var result = _validator.Validate(itemToTest);

            Assert.That(result.WasOperationSuccessful, Is.False);
        }


        [Test]
        public void Validate_ValidItemGiven_ReturnsTrue()
        {
            var itemToTest = new TodoListItem
            {
                Id = Guid.Empty,
                Text = "    hello ",
                CreationTime = new DateTime(2018, 01, 01),
                LastUpdateTime = new DateTime(2018, 01, 01)
            };

            var result = _validator.Validate(itemToTest);

            Assert.That(result.WasOperationSuccessful, Is.True);
            Assert.That(result.Item, Is.EqualTo(itemToTest).UsingItemEqualityComparer());
        }
    }
}