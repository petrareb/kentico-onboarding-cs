using System;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable UnusedVariable

namespace MyOnboardingApp.Contracts.Tests.Validation
{
    [TestFixture]
    public class ResolvedItemTests
    {
        [Test]
        public void Create_NullItemSpecified_ReturnsInvalidResolvedItem()
        {
            var testItem = (TodoListItem)null;

            var result = ResolvedItem.Create(testItem);

            Assert.That(result.WasOperationSuccessful, Is.False);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var itemFromResult = result.Item;
            });
        }


        [Test]
        public void Create_ValidItemSpecified_ReturnsResolvedItemWithCorrectItem()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = string.Empty,
                CreationTime = new DateTime(2000, 01, 01),
                LastUpdateTime = new DateTime(2000, 01, 01)
            };
            var expectedResult = ItemVariantsFactory
                .CreateItemVariants(testItem)
                .ResolvedItem;

            var result = ResolvedItem.Create(testItem);

            Assert.That(result, Is.EqualTo(expectedResult).UsingResolvedItemEqualityComparer());
        }
    }
}