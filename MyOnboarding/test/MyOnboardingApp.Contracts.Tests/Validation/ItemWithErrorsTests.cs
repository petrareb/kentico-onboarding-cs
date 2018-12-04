using System;
using System.Collections.Generic;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UnusedVariable
// ReSharper disable ExpressionIsAlwaysNull

namespace MyOnboardingApp.Contracts.Tests.Validation
{
    [TestFixture]
    public class ItemWithErrorsTests
    {
        [Test]
        public void Create_ItemAndErrorCollectionSpecified_ReturnsValidatedItem()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = string.Empty,
                CreationTime = new DateTime(2000, 01, 01),
                LastUpdateTime = new DateTime(2000, 01, 01)
            };
            var error = new Error(ErrorCode.DataValidationError, "Text is empty.", "text");
            var errors = new List<Error> { error };
            var expectedResult = ItemVariantsFactory
                .CreateItemVariants(testItem, errors)
                .ItemWithErrors;

            var result = ItemWithErrors.Create(testItem, errors);

            Assert.That(result.WasOperationSuccessful, Is.EqualTo(false));
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public void Create_ItemAndEmptyErrorCollectionSpecified_ReturnsValidatedItem()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = "Text",
                CreationTime = new DateTime(2001, 01, 01),
                LastUpdateTime = new DateTime(2001, 01, 01)
            };
            var errors = new List<Error>();
            var expectedResult = ItemVariantsFactory
                .CreateItemVariants(testItem, errors)
                .ItemWithErrors;

            var result = ItemWithErrors.Create(testItem, errors);

            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public void Create_NullItemSpecified_ThrowsArgumentNullException()
        {
            var testItem = (TodoListItem)null;
            var errors = new List<Error>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = ItemWithErrors.Create(testItem, errors);
            });
        }
    }
}