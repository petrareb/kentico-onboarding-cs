using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TrimmedTextNonEmptyCriterionTests
    {
        private IValidationCriterion<TodoListItem> _textCriterion;
        private readonly IErrorFactory _errorFactory = Substitute.For<IErrorFactory>();


        [SetUp]
        public void SetUp()
            => _textCriterion = new TrimmedTextNonEmptyCriterion(_errorFactory);


        [Test]
        public void Validate_ItemWithValidTextSpecified_ReturnsCollectionWithoutErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Text = "text",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }


        [Test]
        public void Validate_ItemWithNullTextSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                Text = null,
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(new List<Error> { error, error, error }));
        }


        [Test]
        public void Validate_ItemWithEmptyTextSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                Text = string.Empty,
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(new List<Error> { error, error }));
        }


        [Test]
        public void Validate_ItemWithTextOfSpacesSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000004"),
                Text = "     ",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithTextOfTabsSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000005"),
                Text = "                ",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithTextWithNewLineSpecified_ReturnsCollectionWithErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Text = "\n",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(new List<Error> { error }));
        }


        [Test]
        public void Validate_ItemWithTextWithNotOnlyWhiteSpacesSpecified_ReturnsCollectionWithoutErrors()
        {
            var testItem = new TodoListItem
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Text = "\naaa   ",
                CreationTime = new DateTime(2015, 01, 01),
                LastUpdateTime = new DateTime(2015, 02, 02)
            };
            var error = new Error(ErrorCode.DataValidationError, "error happened", "error.location");
            _errorFactory.CreateValidationError(Arg.Any<Expression<Func<object>>>(), Arg.Any<string>()).Returns(error);

            var result = _textCriterion.Validate(testItem);

            Assert.That(result, Is.EqualTo(new List<Error>()));
        }
    }
}