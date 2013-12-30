using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {

        [Test]
        public void index_should_return_home_stats_model()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();
            
            // Act
            var controller = new HomeController(authService.Object, demoService.Object);
            var view = controller.Index();

            // Assert
            Assert.NotNull(view.Model);
            
            var model = view.Model as HomeStatsModel;
            Assert.NotNull(model);
            Assert.IsTrue(controller.HasGetAttribute("Index", new[] { typeof(HomeController) }), "HttpGet attribute not found on AppController's Index() action method");

        }

    }
}