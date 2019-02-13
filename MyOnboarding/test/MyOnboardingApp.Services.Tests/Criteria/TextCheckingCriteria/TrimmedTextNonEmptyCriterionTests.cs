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
    public class TrimmedTextNonEmptyCriterionTests
    {
        private IValidationCriterion<TodoListItem> _textCriterion;
       

        [SetUp]
        public void SetUp()
            => _textCriterion = new TrimmedTextNonEmptyCriterion();


        [Test]
        public void Validate_ItemWithNullTextSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000002"),
                text: null,
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocations = new[]
            {
                nameof(testItem.Text),
                nameof(testItem.Text),
                nameof(testItem.Text),
            };


            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EquivalentTo(expectedErrorLocations));
        }


        [Test]
        public void Validate_ItemWithTextOfTabsSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000005"),
                text: "                ",
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocations = new[]
            {
                nameof(testItem.Text)
            };
           
            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(expectedErrorLocations));
        }


        [Test]
        public void Validate_ItemWithTextWithNewLineSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000006"),
                text: "\n",
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocations = new[]
            {
                nameof(testItem.Text)
            };
           
            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EquivalentTo(expectedErrorLocations));
        }

      
        [Test]
        public void Validate_ItemWithTextWithNotOnlyWhiteSpacesSpecified_ReturnsCollectionWithoutErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000007"),
                text: "\naaa   ",
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocations = new string[] {};

            var result = _textCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EqualTo(expectedErrorLocations));
        }
    }
}