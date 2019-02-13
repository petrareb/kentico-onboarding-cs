using System;
using System.Linq;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Criteria.DateCheckingCriteria;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Criteria.DateCheckingCriteria
{
    [TestFixture]
    public class LastUpdateDateBiggerThanCreationDateCriterionTests
    {
        private IValidationCriterion<TodoListItem> _dateCriterion;


        [SetUp]
        public void SetUp()
            => _dateCriterion = new LastUpdateDateBiggerThanCreationDateCriterion();


        [Test]
        public void Validate_ValidItemSpecified_ReturnsEmptyCollection()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000001"),
                text: "test item",
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2015, 02, 02)
            ).Item;
            var expectedErrorLocation = new string[] { };

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_CreationTimeBiggerThanLastUpdateTimeSpecified_ReturnsCollectionWithError()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000002"),
                text: "test",
                creationTime: new DateTime(2015, 01, 01),
                lastUpdateTime: new DateTime(2010, 01, 01)
                ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.CreationTime)
            };

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_CreationTimeSameAsLastUpdateTimeSpecified_ReturnsEmptyCollection()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000003"),
                text: "test",
                creationTime: new DateTime(2010, 01, 01),
                lastUpdateTime: new DateTime(2010, 01, 01)
            ).Item;
            var expectedErrorLocation = new string[] {};

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_CreationTimeIsMinValue_ReturnsCollectionWithError()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000004"),
                text: "test",
                creationTime: DateTime.MinValue,
                lastUpdateTime: new DateTime(2010, 01, 01)
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.CreationTime)
            };

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_CreationTimeAndLastUpdateTimeAreMaxValue_ReturnsCollectionWithErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000005"),
                text: "test",
                creationTime: DateTime.MaxValue,
                lastUpdateTime: DateTime.MaxValue
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.CreationTime),
                nameof(testItem.LastUpdateTime)
            };

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }


        [Test]
        public void Validate_CreationTimeAndLastUpdateTimeAreMinValue_ReturnsCollectionWithErrors()
        {
            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000006"),
                text: "test",
                creationTime: DateTime.MinValue,
                lastUpdateTime: DateTime.MinValue
            ).Item;
            var expectedErrorLocation = new[]
            {
                nameof(testItem.CreationTime),
                nameof(testItem.LastUpdateTime)
            };

            var result = _dateCriterion
                .GetErrorLocationsUsingFakeErrorFactory(testItem)
                .ToArray();

            Assert.That(result, Is.EquivalentTo(expectedErrorLocation));
        }
    }
}