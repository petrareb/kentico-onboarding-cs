using System;
using System.Collections.Generic;
using System.Linq;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.TestUtils.Factories;
using MyOnboardingApp.Validation.Validators;
using NSubstitute;
using NUnit.Framework;


namespace MyOnboardingApp.Validation.Tests.Validators
{
    public class ValidatorTests
    {
        private Validator<TodoListItem> _validator;


        [Test]
        public void Validate_TwoCriteriaNotFulfilled_ReturnsCorrectItemWithErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            _validator = new Validator<TodoListItem>(new[] { criterion1, criterion2 });

            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };
            IEnumerable<Error> criterion1Errors = new List<Error> { new Error(ErrorCode.DataValidationError, "validation message", "validation.location") };
            criterion1.Validate(testItem).Returns(criterion1Errors);
            IEnumerable<Error> criterion2Errors = new List<Error> { new Error(ErrorCode.PermissionError, "permission message", "permission.location") };
            criterion2.Validate(testItem).Returns(criterion2Errors);
            var errors = new List<Error>(criterion1Errors.Concat(criterion2Errors));
            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem, errors).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem);
            criterion2.Received(1).Validate(testItem);
            Assert.That(result.Errors, Is.EqualTo(errors));
            Assert.That(result.WasOperationSuccessful, Is.EqualTo(expectedResult.WasOperationSuccessful));
        }


        [Test]
        public void Validate_AllCriteriaFulfilled_ReturnsCorrectItemWithoutErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            _validator = new Validator<TodoListItem>(new[] { criterion1, criterion2 });
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = "test",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };

            criterion1.Validate(testItem).Returns(new List<Error>());
            criterion2.Validate(testItem).Returns(new List<Error>());
            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem, new List<Error>()).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem);
            criterion2.Received(1).Validate(testItem);
            Assert.That(result.WasOperationSuccessful, Is.EqualTo(expectedResult.WasOperationSuccessful));
            Assert.That(result.Item, Is.EqualTo(expectedResult.Item));
            Assert.That(result.Errors, Is.EqualTo(expectedResult.Errors));
        }


        [Test]
        public void Validate_OneCriterionNotFulfilled_ReturnsCorrectItemWithoutErrors()
        {
            var criterion1 = Substitute.For<IValidationCriterion<TodoListItem>>();
            var criterion2 = Substitute.For<IValidationCriterion<TodoListItem>>();
            _validator = new Validator<TodoListItem>(new[] { criterion1, criterion2 });
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = "test",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };
            var criterion1Errors = new List<Error>
            {
                new Error(ErrorCode.DataValidationError, "validation message", "validation.location")
            };
            // ReSharper disable once CollectionNeverUpdated.Local
            var criterion2Errors = new List<Error>();
            criterion1.Validate(testItem).Returns(criterion1Errors);
            criterion2.Validate(testItem).Returns(criterion2Errors);
            var errors = new List<Error>(criterion1Errors.Concat(criterion2Errors));

            var expectedResult = ItemVariantsFactory.CreateItemVariants(testItem, errors).ItemWithErrors;

            var result = _validator.Validate(testItem);

            criterion1.Received(1).Validate(testItem);
            criterion2.Received(1).Validate(testItem);
            Assert.That(result.WasOperationSuccessful, Is.EqualTo(expectedResult.WasOperationSuccessful));
            Assert.That(result.Item, Is.EqualTo(expectedResult.Item));
            Assert.That(result.Errors, Is.EqualTo(expectedResult.Errors));
        }
    }
}