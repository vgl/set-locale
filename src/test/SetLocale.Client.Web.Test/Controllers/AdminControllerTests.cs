using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
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
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value)).Returns(() => Task.FromResult<int?>(1));

            // Act
            var controller = new AdminController(userService.Object, null, null);
            var view = await controller.NewTranslator(inValidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            var model = view.Model as UserModel;

            Assert.NotNull(model);

            controller.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
        }

        [Test]
        public void users_id_is_greater_four_should_return_with_app_model()
        {
            // Arrange           
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetAll()).Returns(() => Task.FromResult<List<User>>(new List<User>())); 
             
            // Act
            var controller = new AdminController(userService.Object, null, null);
            var view = controller.Users(5);     

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Users",new[] { typeof(int) });
            userService.Verify(x => x.GetAll(), Times.Once);
             
        }

        [Test]
        public void users_id_is_between_one_and_four_should_return_with_app_model()
        {
            // Arrange           
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetAllByRoleId(1)).Returns(() => Task.FromResult<List<User>>(new List<User>()));

            // Act
            var controller = new AdminController(userService.Object, null, null);
            var view = controller.Users(1);      

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Users", new[] { typeof(int) });
            userService.Verify(x => x.GetAllByRoleId(1), Times.Once);

        }

        [Test]
        public void apps_should_return_with_app_model()
        {
            // Arrange           
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.GetAll()).Returns(() => Task.FromResult<List<App>>(new List<App>()));

            // Act
            var controller = new AdminController(null, null, appService.Object);
            var view = controller.Apps();

            // Assert
            Assert.NotNull(view);
            controller.AssertGetAttribute("Apps");
            appService.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
