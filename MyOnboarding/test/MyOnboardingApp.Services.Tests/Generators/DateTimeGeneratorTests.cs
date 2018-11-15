using System;
using MyOnboardingApp.Services.Generators;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Generators
{
    [TestFixture]
    public class DateTimeGeneratorTests
    {
        private readonly DateTimeGenerator _generator = new DateTimeGenerator();


        [Test]
        public void GetCurrentDateTime_CalledTwoTimes_ReturnsSameDate()
        {
            var date1 = _generator.GetCurrentDateTime().Date;
            var date2 = _generator.GetCurrentDateTime().Date;

            Assert.That(date1, Is.EqualTo(date2));
        }


        [Test]
        public void GetCurrentDateTime_DoesNotReturnMinDateTime()
        {
            var dateTime = _generator.GetCurrentDateTime();

            Assert.That(dateTime, Is.Not.EqualTo(DateTime.MinValue));
        }


        [Test]
        public void GetCurrentDateTime_ReturnsCurrentDateTime()
        {
            var dateTime = _generator.GetCurrentDateTime();

            var expectedDateTime = DateTime.UtcNow;

            Assert.That(dateTime, Is.EqualTo(expectedDateTime).Within(0.1).Percent);
        }
    }
}