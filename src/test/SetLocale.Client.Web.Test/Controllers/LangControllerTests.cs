using System.Web;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Test.Builders;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class LangControllerTests
    {
        [Test]
        public void change_should_add_lang_cookie()
        {
            // Arrange
            const string actionName = "Change";

            var controllerContext = new Mock<ControllerContext>();
            var httpContext = new Mock<HttpContextBase>();
            var httpRequest = new Mock<HttpRequestBase>();
            var httpResponse = new Mock<HttpResponseBase>();

            controllerContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            httpResponse.Setup(x => x.SetCookie(It.IsAny<HttpCookie>()));

            // Act
            var sut = new LangControllerBuilder().Build();
            sut.ControllerContext = controllerContext.Object;

            var view = sut.Change("tr");

            // Assert
            Assert.NotNull(view);
            
            sut.AssertGetAttribute(actionName, new[] { typeof(string) });
            sut.AssertAllowAnonymousAttribute(actionName, new[] { typeof(string) });

            httpResponse.Verify(x => x.SetCookie(It.IsAny<HttpCookie>()), Times.AtLeastOnce);
        }
    }
}