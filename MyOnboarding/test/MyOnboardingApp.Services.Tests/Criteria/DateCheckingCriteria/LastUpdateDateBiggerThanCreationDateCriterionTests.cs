using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Criteria.DateCheckingCriteria;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Criteria.DateCheckingCriteria
{
    [TestFixture]
    public class LastUpdateDateBiggerThanCreationDateCriterionTests
    {
        private IValidationCriterion<TodoListItem> _dateCriterion;
        private readonly IErrorFactory _errorFactory = Substitute.For<IErrorFactory>();


        [SetUp]
        public void SetUp()
            => _dateCriterion = new LastUpdateDateBiggerThanCreationDateCriterion();


        [Test]
        public void Validate_ValidItemSpecified_ReturnsEmptyCollection()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "test",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _dateCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }


        [Test]
        public void Validate_CreationTimeBiggerThanLastUpdateTimeSpecified_ReturnsCollectionWithError()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = "test",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _dateCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_CreationTimeSameAsLastUpdateTimeSpecified_ReturnsEmptyCollection()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                Text = "test",
                CreationTime = new DateTime(2010, 01, 01),
                LastUpdateTime = new DateTime(2010, 01, 01)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _dateCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }
    }
}