using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Models;
using System.Web.Mvc;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AppControllerTests
    {
        [Test]
        public void index_should_return_app_model()
        {
            // Arrange           
            var demoService = new Mock<IDemoDataService>();
            // Act
            var controller = new AppController(null, demoService.Object);
            var view = controller.Index();
            // Assert
            Assert.NotNull(view);
            demoService.Verify(x => x.GetAnApp(), Times.Once);
             
        }
        [Test]
        public void index_should_return_new_app_model_valid()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();

            var valid = new AppModel { Name = "test",Url="test.com",Description="test description" };
            demoService.Setup(x => x.GetAnApp()).Returns(valid);
            // Act
            var controller = new AppController(authService.Object, demoService.Object);
            var view = controller.New(valid) as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model as AppModel;
            Assert.NotNull(model);
            demoService.Verify(x => x.GetAnApp(), Times.Once);

        }
        [Test]
        public void index_should_return_new_app_model_invalid()
        {
            // Arrange
            var authService = new Mock<IFormsAuthenticationService>();
            var demoService = new Mock<IDemoDataService>();
            var invalid = new AppModel { Name = "test", Url = "test.com" };
            demoService.Setup(x => x.GetAnApp()).Returns(invalid);

            // Act
            var controller = new AppController(authService.Object, demoService.Object);
           
            // Assert
           var  redirectResult =controller.New(invalid) as RedirectResult ;
            Assert.NotNull(redirectResult);
            //Assert
            Assert.AreEqual(redirectResult.Url, "/user/apps");
            demoService.Verify(x => x.GetAnApp(), Times.Once);

        }

    }


}
