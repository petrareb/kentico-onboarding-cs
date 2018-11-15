using System;
using MyOnboardingApp.Services.Generators;
using NUnit.Framework;

namespace MyOnboardingApp.Services.Tests.Generators
{
    [TestFixture]
    public class GuidGeneratorTests
    {
        private readonly GuidGenerator _generator = new GuidGenerator();


        [Test]
        public void GetNewId_DoesNotReturnEmptyGuid()
        {
            var id = _generator.GetNewId();

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }


        [Test]
        public void GetNewId_CalledMoreTimes_DoesNotReturnSameIds()
        {
            var id1 = _generator.GetNewId();
            var id2 = _generator.GetNewId();

            Assert.That(id1, Is.Not.EqualTo(id2));
        }
    }
}