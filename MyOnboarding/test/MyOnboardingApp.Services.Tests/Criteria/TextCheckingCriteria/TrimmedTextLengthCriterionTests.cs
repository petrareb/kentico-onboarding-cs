using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyOnboardingApp.Contracts.ErrorFactories;
using MyOnboardingApp.Contracts.Errors;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;
using MyOnboardingApp.Services.Criteria.TextCheckingCriteria;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Criteria.TextCheckingCriteria
{
    [TestFixture]
    public class TrimmedTextLengthCriterionTests
    {
        private IValidationCriterion<TodoListItem> _textCriterion;
        private readonly IErrorFactory _errorFactory = Substitute.For<IErrorFactory>();


        [SetUp]
        public void SetUp()
            => _textCriterion = new TrimmedTextLengthCriterion();


        [Test]
        public void Validate_ItemWithShorterTextSpecified_ReturnsCollectionWithError()
        {
            const int length = TrimmedTextLengthCriterion.MinLength == 0
                // ReSharper disable once UnreachableCode
                ? 0
                : TrimmedTextLengthCriterion.MinLength - 1;
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = new string('a', length),
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "text");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithLongerTextSpecified_ReturnsCollectionWithError()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = new string('a', TrimmedTextLengthCriterion.MaxLength + 1),
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem,_errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithEmptyTextSpecified_ReturnsCollectionWithError()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = string.Empty,
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithMinimalTextSpecified_ReturnsEmptyErrorCollection()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = new string('a', TrimmedTextLengthCriterion.MinLength),
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }


        [Test]
        public void Validate_ItemWithMaximalTextSpecified_ReturnsEmptyErrorCollection()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = new string('a', TrimmedTextLengthCriterion.MaxLength),
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem, _errorFactory);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }
    }
}