using System;
using System.Web.Http.Routing;
using MyOnboardingApp.Api.UrlLocation;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.UrlLocation;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.ApiServices.Tests
{
    [TestFixture]
    public class ItemUrlLocatorTest
    {
        private IUrlLocator _locator;
        private UrlHelper _urlHelper;

        [SetUp]
        public void SetUp()
        {
            _urlHelper = Substitute.For<UrlHelper>();
            var itemLocatorConfig = new UrlLocatorConfig(/*"ListItemUrl"*/);
            _locator = new ItemUrlLocator(_urlHelper, itemLocatorConfig);
        }

        [Test]
        public void GetListItemUrl_IdSpecified_ReturnsCorrectUrlWithId()
        {
            var testId = Guid.Empty; 
            var routePrefix = "testRoute/";
            _urlHelper.Route(Arg.Any<string>(), Arg.Is<object>(obj => (Guid) new HttpRouteValueDictionary(obj)["id"] == testId)).ReturnsForAnyArgs(x => routePrefix + testId);
            var resultUrl = _locator.GetListItemUrl(testId);

            Assert.That(resultUrl.EndsWith(testId.ToString()));
            Assert.That(resultUrl.Contains(routePrefix));
        }
    }
}
