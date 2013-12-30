using System.Web;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class LangControllerTests
    {
        [Test]
        public void change_should_add_lang_cookie()
        {
            // Arrange
            var controllerContext = new Mock<ControllerContext>();
            var httpContext = new Mock<HttpContextBase>();
            controllerContext.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var httpRequest = new Mock<HttpRequestBase>();
            var httpResponse = new Mock<HttpResponseBase>();
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            httpResponse.Setup(x => x.SetCookie(It.IsAny<HttpCookie>()));
            
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();
            
            // Act
            var controller = new LangController(authService.Object, demoService.Object);
            controller.ControllerContext = controllerContext.Object;
            
            var view = controller.Change("tr");

            // Assert
            Assert.NotNull(view);
        }
    }
}