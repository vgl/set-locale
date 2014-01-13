using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AdminControllerTests
    {
        [Test]
        public void index_should_return()
        {
            // Act
            var controller = new AdminController(null, null, null);
            var view = controller.Index();

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Index");
        }

        [Test]
        public void new_translator_should_return_with_user_model()
        {
            // Act
            var controller = new AdminController(null, null, null);
            var view = controller.NewTranslator();
            var model = view.Model;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);
            controller.AssertGetAttribute("NewTranslator");
        }

        [Test]
        public async void new_translator_should_redirect_if_model_is_valid()
        {
            // Arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value)).Returns(() => Task.FromResult<int?>(1));

            // Act
            var controller = new AdminController(userService.Object, null, null);
            var view = await controller.NewTranslator(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/admin/users");
            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value), Times.Once);
            controller.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
        }

        [Test]
        public async void new_translator_should_return_with_app_model_if_model_is_invalid()
        {
            // Arrange
            var inValidModel = new UserModel { Name = "test name" };

            // Act
            var controller = new AdminController(null, null, null);
            var view = await controller.NewTranslator(inValidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(UserModel), view.Model);
            controller.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
        }

        //[Test]
        //public async void users_should_return_with_list_user_model()
        //{
        //    // Arrange           
        //    var userService = new Mock<IUserService>();
        //    userService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<User>()));
             
        //    // Act
        //    var controller = new AdminController(userService.Object, null, null);
        //    var view = await controller.Users(5) as ViewResult;     

        //    // Assert
        //    Assert.NotNull(view);
        //    Assert.NotNull(view.Model);
        //    Assert.IsAssignableFrom(typeof(List<UserModel>), view.Model);
        //    controller.AssertGetAttribute("Users", new[] { typeof(int) });
        //    userService.Verify(x => x.GetAll(), Times.Once); 
        //}
         
        [Test]
        public async void apps_should_return_with_list_app_model()
        {
            // Arrange           
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<App>()));

            // Act
            var controller = new AdminController(null, null, appService.Object);
            var view = await controller.Apps() as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(List<AppModel>), view.Model); 
            controller.AssertGetAttribute("Apps");
            appService.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
