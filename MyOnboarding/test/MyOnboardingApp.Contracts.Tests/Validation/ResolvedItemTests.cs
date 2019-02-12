using System;
using System.Diagnostics.CodeAnalysis;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

namespace MyOnboardingApp.Contracts.Tests.Validation
{
    [TestFixture]
    public class ResolvedItemTests
    {
        [Test]
        public void Create_NullItemSpecified_ReturnsInvalidResolvedItem()
        {
            var testItem = (TodoListItem)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = ResolvedItem.Create(testItem);

            Assert.Multiple(() =>
            {
                Assert.That(result.WasOperationSuccessful, Is.False);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var _ = result.Item;
                });
            });
        }


        [Test]
        public void Create_ValidItemSpecified_ReturnsResolvedItemWithCorrectItem()
        {
            var (resolvableItem, expectedResolvedItem, _) = ItemVariantsFactory.CreateItemVariants(
                id: "00000000-0000-0000-0000-000000000001",
                text: string.Empty,
                creationTime: "2001-01-01",
                errors: null
                );

            var actualResolvedItem = ResolvedItem.Create(resolvableItem);

            Assert.That(actualResolvedItem, Is.EqualTo(expectedResolvedItem).UsingResolvedItemEqualityComparer());
        }


        [Test]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void Create_NullItemSpecified_ReturnsTheSameInvalidResolvedItem()
        {
            var testItem = (TodoListItem)null;

            var result1 = ResolvedItem.Create(testItem);
            var result2 = ResolvedItem.Create(testItem);

            Assert.That(result1, Is.EqualTo(result2).UsingResolvedItemEqualityComparer());
        }
    }
}