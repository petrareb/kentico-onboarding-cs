using System;
using System.Web.Http.Routing;
using MyOnboardingApp.ApiServices.UrlLocation;
using MyOnboardingApp.Contracts.Urls;
using NSubstitute;
using NUnit.Framework;

namespace MyOnboardingApp.ApiServices.Tests.UrlLocation
{
    [TestFixture]
    public class ItemUrlLocatorTests
    {
        [Test]
        public void GetListItemUrl_IdSpecified_ReturnsCorrectUrlWithId()
        {
            var testId = new Guid("11111111-1111-1111-1111-aabbccddeeff");
            var locator = CreateUrlLocator(testId);

            var resultUrl = locator.GetListItemUrl(testId);

            Assert.That(resultUrl.Equals(testId.ToString()));
        }


        private static IUrlLocator CreateUrlLocator(Guid testId)
        {
            const string routeName = "RouteName";

            var locatorConfig = Substitute.For<IRoutesConfig>();
            locatorConfig.TodoListItemRouteNameGetter.Returns(routeName);

            var urlHelper = CreateUrlHelper(testId, routeName);

            return new ItemUrlLocator(urlHelper, locatorConfig);
        }


        private static UrlHelper CreateUrlHelper(Guid testId, string routeName)
        {
            var urlHelper = Substitute.For<UrlHelper>();
            urlHelper
                .Route(
                    routeName,
                    Arg.Is<object>(obj => (Guid)new HttpRouteValueDictionary(obj)["id"] == testId))
                .Returns(testId.ToString());

            return urlHelper;
        }
    }
}