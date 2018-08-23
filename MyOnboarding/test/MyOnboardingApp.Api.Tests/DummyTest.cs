using NUnit.Framework;

namespace MyOnboardingApp.Tests
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void AddingTest()
        {
            const int num1 = 5;
            const int num2 = 8;
            const int expectedResult = 13;

            const int result = num1 + num2;

            Assert.AreEqual(expectedResult, result);
        }
    }
}
