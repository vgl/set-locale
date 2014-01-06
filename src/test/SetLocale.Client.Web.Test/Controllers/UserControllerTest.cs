using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Moq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks.Constraints;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public async void apps_should_return_with_app_model()
        {
            // Arrange
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.GetByUserId(1)).Returns(() => Task.FromResult(new List<App>()));

            // Act
            var controller = new UserController(null, null, null, appService.Object);
            var view = await controller.Apps(1) as ViewResult;

            // Assert
            Assert.NotNull(view);

            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom<List<AppModel>>(model);
            controller.AssertGetAttribute("Apps", new[] { typeof(int) });

            appService.Verify(x => x.GetByUserId(1), Times.Once);

        }

        [Test]
        public async void words_should_return_with_word_model()
        {
            // Arrange
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByUserId(1)).Returns(() => Task.FromResult(new List<Word>()));

            // Act
            var controller = new UserController(null, wordService.Object, null, null);
            var view = await controller.Words(1) as ViewResult;

            // Assert
            Assert.NotNull(view);

            var model = view.Model;
            Assert.NotNull(model);
            controller.AssertGetAttribute("Words", new[] { typeof(int) });

            wordService.Verify(x => x.GetByUserId(1), Times.Once);

        }

        [Test]
        public async void change_status_should_return_with_response_model()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.ChangeStatus(1, true)).Returns(() => Task.FromResult(new bool()));

            // Act
            var controller = new UserController(userService.Object, null, null, null);
            var view = await controller.ChangeStatus(1, true);

            // Assert
            Assert.NotNull(view);

            var model = view.Data;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(ResponseModel), model);

            controller.AssertPostAttribute("ChangeStatus", new[] { typeof(int), typeof(bool) });
            userService.Verify(x => x.ChangeStatus(1, true), Times.Once);

        }

        [Test]
        public void new_should_return_with_user_model()
        {
            // Act
            var controller = new UserController(null, null, null, null);
            var view = controller.New() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);
            controller.AssertGetAttribute("New");
            controller.AssertAllowAnonymousAttribute("New");
        }

        [Test]
        public async void new_should_redirect_if_model_is_valid()
        {
            // Arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com", Password = "pass" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value)).Returns(() => Task.FromResult<int?>(1));

            // Act
            var controller = new UserController(userService.Object, null, null, null);
            var view = await controller.New(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value), Times.Once);
            controller.AssertPostAttribute("New", new[] { typeof(UserModel) });
            controller.AssertAllowAnonymousAttribute("New", new[] { typeof(UserModel) });

        }

        [Test]
        public async void new_should_return_with_user_model_if_model_is_invalid()
        {
            // Arrange
            var invalidModel = new UserModel { Name = "test name" };

            // Act
            var controller = new UserController(null, null, null, null);
            var view = await controller.New(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);
            controller.AssertPostAttribute("New", new[] { typeof(UserModel) });
            controller.AssertAllowAnonymousAttribute("New", new[] { typeof(UserModel) });

        }

        [Test]
        public void reset_should_return_with_reset_model()
        {
            // Act
            var controller = new UserController(null, null, null, null);
            var view = controller.Reset() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(ResetModel), model);
            controller.AssertGetAttribute("Reset");
            controller.AssertAllowAnonymousAttribute("Reset");
        }

        [Test]
        public void login_should_return_with_login_model()
        {
            // Act
            var controller = new UserController(null, null, null, null);
            var view = controller.Login() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(LoginModel), model);
            controller.AssertGetAttribute("Login");
            controller.AssertAllowAnonymousAttribute("Login");

        }

        [Test]
        public async void login_should_redirect_if_model_is_valid()
        {
            // Arrange   
            const int id = 1;
            const string email = "test@test.com"; 
            const string name = "test";
            var validModel = new LoginModel { Email = email, Password = "pass" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Authenticate(validModel.Email,validModel.Password)).Returns(() => Task.FromResult(true));
            userService.Setup(x => x.GetByEmail(validModel.Email))
                                    .Returns(() => Task.FromResult(new User
                                                                    { Id = id ,
                                                                      Name = name,
                                                                      Email = email
                                                                    }));

            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignIn(string.Format("{0}|{1}|{2}", id, name, email), true));    

            // Act
            var controller = new UserController(userService.Object, null, formsAuthenticationService.Object, null);
            var view = await controller.Login(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            userService.Verify(x => x.Authenticate(validModel.Email, validModel.Password), Times.Once);
            controller.AssertPostAttribute("Login", new[] { typeof(LoginModel) });
            controller.AssertAllowAnonymousAttribute("Login", new[] { typeof(LoginModel) });
        }

        [Test]
        public async void login_should_return_with_login_model_if_model_is_invalid()
        {
            // Arrange
            var invalidModel = new LoginModel { Email = "test@test.com"};   

            // Act
            var controller = new UserController(null, null, null, null);
            var view = await controller.Login(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(LoginModel), model); 
            controller.AssertPostAttribute("Login", new[] { typeof(LoginModel) });
            controller.AssertAllowAnonymousAttribute("Login", new[] { typeof(LoginModel) });

        }

        [Test]
        public async void logout_should_redirect()
        {
            // Arrange    
            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignOut());

            // Act
            var controller = new UserController(null, null, formsAuthenticationService.Object, null);
            var view = controller.Logout() as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/home/index");
            controller.AssertGetAttribute("Logout");  
        }

    }
}
