using System;
using System.Linq;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Criteria.TextCheckingCriteria;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Criteria.TextCheckingCriteria
{
    [TestFixture]
    public class TrimmedTextLengthCriterionTests
    {
        private IValidationCriterion<TodoListItem> _textCriterion;


        [SetUp]
        public void SetUp()
            => _textCriterion = new TrimmedTextLengthCriterion();


        [Test]
        public void Validate_ItemWithShorterTextSpecified_ReturnsCollectionWithError()
        {
            const int length = TrimmedTextLengthCriterion.MinLength - 1;
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000001"),
                text: new string('a', length),
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.Text)
            };

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_ItemWithLongerTextSpecified_ReturnsCollectionWithError()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000002"),
                text: new string('a', TrimmedTextLengthCriterion.MaxLength + 1),
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.Text)
            };

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_ItemWithEmptyTextSpecified_ReturnsCollectionWithError()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000003"),
                text: string.Empty,
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.Text)
            };

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_ItemWithMinimalTextSpecified_ReturnsEmptyErrorCollection()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000004"),
                text: new string('a', TrimmedTextLengthCriterion.MinLength),
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocations = new string[] { };

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocations));
        }


        [Test]
        public void Validate_ItemWithMaximalTextSpecified_ReturnsEmptyErrorCollection()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000005"),
                text: new string('a', TrimmedTextLengthCriterion.MaxLength),
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocation = new string[] { };

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }
    }
}