using System;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.Urls;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.ApiServices.Tests.UrlLocation
{
    [TestFixture]
    public class ItemUrlLocatorTests
    {
        private IUrlLocator _locator;
        private UrlHelper _urlHelper;


        [SetUp]
        public void SetUp()
        {
            _urlHelper = Substitute.For<UrlHelper>();
            var locatorConfig = new UrlLocatorConfig();
            _locator = new ItemUrlLocator(_urlHelper, locatorConfig);
        }


        [Test]
        public void GetListItemUrl_IdSpecified_ReturnsCorrectUrlWithId()
        {
            var testId = new Guid("11111111-1111-1111-1111-aabbccddeeff");
            _urlHelper
                .Route(
                    UrlLocatorConfig.TodoListItemRouteName,
                    Arg.Is<object>(obj => (Guid) new HttpRouteValueDictionary(obj)["id"] == testId))
                .Returns(testId.ToString());

            var resultUrl = _locator.GetListItemUrl(testId);

            Assert.That(resultUrl.Equals(testId.ToString()));
        }
    }
}