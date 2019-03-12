using System;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

namespace MyOnboardingApp.Contracts.Tests.Validation
{
    [TestFixture]
    public class ItemWithErrorsTests
    {
        [Test]
        public void Create_ItemAndErrorCollectionSpecified_ReturnsInvalidatedItem()
        {
            var error = new Error(ErrorCode.DataValidationError, "Text is empty.", "text");
            var errors = new [] { error };
            var (testItem, _, expectedResult) = ItemVariantsFactory
                .CreateItemVariants(
                    new Guid("00000000-0000-0000-0000-000000000001"),
                    string.Empty,
                    new DateTime(2000, 01, 01),
                    errors
                );

            var result = ItemWithErrors.Create(testItem, errors);

            Assert.Multiple(() =>
            {
                Assert.That(result.WasOperationSuccessful, Is.False);
                Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
            });
        }


        [Test]
        public void Create_ItemAndEmptyErrorCollectionSpecified_ReturnsValidatedItem()
        {
            var errors = Array.Empty<Error>();
            var (testItem, _, expectedResult) = ItemVariantsFactory
                .CreateItemVariants(
                    new Guid("00000000-0000-0000-0000-000000000002"),
                    "Text",
                    new DateTime(2001, 01, 01),
                    errors
                );

            var result = ItemWithErrors.Create(testItem, errors);

            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public void Create_NullItemSpecified_ThrowsArgumentNullException()
        {
            var testItem = (TodoListItem) null;
            var errors = new Error[] { };

            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                var _ = ItemWithErrors.Create(testItem, errors);
            });
        }


        [Test]
        public void Create_NullItemAndNonEmptyErrorCollectionSpecified_ThrowsArgumentNullException()
        {
            var testItem = (TodoListItem) null;
            var error = new Error(ErrorCode.DataValidationError, "Text is empty.", "text");
            var errors = new[] { error };

            Assert.Throws<ArgumentNullException>(() =>
                {
                    // ReSharper disable once ExpressionIsAlwaysNull
                    var _ = ItemWithErrors.Create(testItem, errors);
                } 
            );
        }
    }
}