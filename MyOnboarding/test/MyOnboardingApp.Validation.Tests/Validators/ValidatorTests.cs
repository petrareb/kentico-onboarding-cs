using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Extensions;
using MyOnboardingApp.TestUtils.Factories;
using MyOnboardingApp.Validation.Validators;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Validation.Tests.Validators
{
    public class ValidatorTests
    {
        private InvariantValidator<TodoListItem> _validator;


        [Test]
        public void Validate_TwoCriteriaNotFulfilled_ReturnsCorrectItemWithErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var errorFactory = Substitute.For<IErrorFactory>();
            _validator = new InvariantValidator<TodoListItem>(new[] { criterion1, criterion2 }, errorFactory);

            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000001"),
                text: string.Empty,
                creationTime: new DateTime(2010, 01, 01)
            ).Item;
            var criterion1Errors = new []
            {
                new Error(ErrorCode.DataValidationError, "validation message", "validation.location")
            };
            criterion1.Validate(testItem, errorFactory).Returns(criterion1Errors);
            var criterion2Errors = new []
            {
                new Error(ErrorCode.PermissionError, "permission message", "permission.location")
            };
            criterion2.Validate(testItem, errorFactory).Returns(criterion2Errors);
            var errors = Enumerable
                .Empty<Error>()
                .Union(criterion1Errors)
                .Union(criterion2Errors)
                .ToArray();
            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem, errors).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem, errorFactory);
            criterion2.Received(1).Validate(testItem, errorFactory);
            Assert.That(result.Errors, Is.EquivalentTo(errors));
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public void Validate_AllCriteriaFulfilled_ReturnsCorrectItemWithoutErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var errorFactory = Substitute.For<IErrorFactory>();
            _validator = new InvariantValidator<TodoListItem>(new[] { criterion1, criterion2 }, errorFactory);

            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000002"),
                text: "test",
                creationTime: new DateTime(2010, 01, 01)
            ).Item;
            criterion1.Validate(testItem, errorFactory).Returns(new List<Error>());
            criterion2.Validate(testItem, errorFactory).Returns(new List<Error>());
            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem, errorFactory);
            criterion2.Received(1).Validate(testItem, errorFactory);
            Assert.That(result.Errors, Is.EqualTo(expectedResult.Errors));
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
        }


        [Test]
        public void Validate_OneCriterionNotFulfilled_ReturnsCorrectItemWithoutErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var errorFactory = Substitute.For<IErrorFactory>();
            _validator = new InvariantValidator<TodoListItem>(new[] { criterion1, criterion2 }, errorFactory);

            var testItem = ItemVariantsFactory.CreateItemVariants(
                id: new Guid("00000000-0000-0000-0000-000000000003"),
                text: "test",
                creationTime: new DateTime(2010, 01, 01)
            ).Item;
            var criterion1Errors = new []
            {
                new Error(ErrorCode.DataValidationError, "validation message", "validation.location")
            };
            var criterion2Errors = new Error[] {};
            criterion1.Validate(testItem, errorFactory).Returns(criterion1Errors);
            criterion2.Validate(testItem, errorFactory).Returns(criterion2Errors);
            var errors = Enumerable
                .Empty<Error>()
                .Union(criterion1Errors)
                .Union(criterion2Errors)
                .ToArray();

            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem, errors).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem, errorFactory);
            criterion2.Received(1).Validate(testItem, errorFactory);
            Assert.That(result, Is.EqualTo(expectedResult).UsingItemWithErrorsEqualityComparer());
            Assert.That(result.Errors, Is.EqualTo(expectedResult.Errors));
        }
    }
}