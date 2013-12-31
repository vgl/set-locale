using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;
using System.Web.Mvc;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AdminControllerTests
    {
        [Test]
        public void index_should_return_user_model()
        {
            // Act
            var controller = new AdminController(null, null);
            var view = controller.Index();

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Index");
            
        }

             [Test]
             public void new_translator_should_return_user_model()
             {
                 // Arrange           
                 var demoService = new Mock<IDemoDataService>();

                 // Act
                 var controller = new AdminController(null, demoService.Object);
                 var view = controller.NewTranslator();

                 // Assert
                 Assert.NotNull(view);
                 controller.AssertGetAttribute("NewTranslator");
                 demoService.Verify(x => x.GetAUser(), Times.Once);
             }

             [Test]
             public void new_translator_should_redirect_if_model_is_valid()
             {
                 // Arrange
                 var validModel = new UserModel { Name = "test name", Email = "test@test.com" };

                 // Act
                 var controller = new AdminController(null, null);
                 var view = controller.NewTranslator(validModel) as RedirectResult;

                 // Assert
                 Assert.NotNull(view);
                 Assert.AreEqual(view.Url, "/admin/users");
                 controller.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
             }

             [Test]
             public void new_translator_should_return_app_model_if_model_is_invalid()
             {
                 // Arrange
                 var inValidModel = new UserModel { Name = "test name" };

                 // Act
                 var controller = new AdminController(null, null);
                 var view = controller.NewTranslator(inValidModel) as ViewResult;

                 // Assert
                 Assert.NotNull(view);
                 Assert.NotNull(view.Model);
                 var model = view.Model as UserModel;

                 Assert.NotNull(model);

                 controller.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
             }

             [Test]
             public void Users_should_return_app_model()
             {
                 // Arrange           
                 var demoService = new Mock<IDemoDataService>();

                 // Act
                 var controller = new AdminController(null, demoService.Object);
                 var view = controller.Users();

                 // Assert
                 Assert.NotNull(view);
                 controller.AssertGetAttribute("Users");
                 demoService.Verify(x => x.GetAllUsers(), Times.Once);
             }

             [Test]
             public void apps_should_return_app_model()
             {
                 // Arrange           
                 var demoService = new Mock<IDemoDataService>();

                 // Act
                 var controller = new AdminController(null, demoService.Object);
                 var view = controller.Apps();

                 // Assert
                 Assert.NotNull(view);
                 controller.AssertGetAttribute("Apps");
                 demoService.Verify(x => x.GetAllApps(), Times.Once);
             }       
    }
}
