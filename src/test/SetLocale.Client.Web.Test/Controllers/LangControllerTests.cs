using System.Web.Mvc;

using Moq;
using NUnit.Framework;
using Rhino.Mocks;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class LangControllerTests
    {
        [Test]
        public void change_should_return_add_lang_cookie()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();
            
            //todo: mock https response

            // Act
            var controller = new LangController(authService.Object, demoService.Object);
            var view = controller.Change("tr");

            // Assert
            Assert.NotNull(view);
        }
         
    }
}