using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AdminControllerTests
    {
        [Test]
        public void admin_index_test()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            //todo: mock https response

            // Act
            var controller = new AdminController(authService.Object, demoService.Object);
            var view = controller.NewTranslator();
            var view2 = controller.Users();
            var view3 = controller.Apps();

            demoService.Verify(x => x.GetAllApps(), Times.Once);
            demoService.Verify(x => x.GetAllUsers(), Times.Once);
            demoService.Verify(x => x.GetAUser(), Times.Once);
            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view2);
            Assert.NotNull(view3);
        }

    }


}
