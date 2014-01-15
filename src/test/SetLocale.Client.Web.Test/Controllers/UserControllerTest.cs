using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        //[Test]
        //public async void apps_should_return_with_app_model()
        //{
        //    // Arrange
        //    var appService = new Mock<IAppService>();
        //    appService.Setup(x => x.GetByUserId(1)).Returns(() => Task.FromResult(new List<App>()));

        //    // Act
        //    var sut = new UserControllerBuilder().WithAppService(appService.Object)
        //                                         .Build();
        //    var view = await sut.Apps(1) as ViewResult;

        //    // Assert
        //    Assert.NotNull(view);

        //    var model = view.Model;
        //    Assert.NotNull(model);
        //    Assert.IsAssignableFrom<List<AppModel>>(model);
        //    sut.AssertGetAttribute("Apps", new[] { typeof(int) });

        //    appService.Verify(x => x.GetByUserId(1), Times.Once);

        //}

        //[Test]
        //public async void words_should_return_with_word_model()
        //{
        //    // Arrange
        //    var wordService = new Mock<IWordService>();
        //    wordService.Setup(x => x.GetByUserId(1)).Returns(() => Task.FromResult(new List<Word>()));

        //    // Act
           
        //    var sut = new UserControllerBuilder().WithWordService(wordService.Object)
        //                                         .Build();

        //    var view = await sut.Words(1) as ViewResult;

        //    // Assert
        //    Assert.NotNull(view);

        //    var model = view.Model;
        //    Assert.NotNull(model);
        //    sut.AssertGetAttribute("Words", new[] { typeof(int) });

        //    wordService.Verify(x => x.GetByUserId(1), Times.Once);
        //}

        [Test]
        public async void change_status_should_return_with_response_model()
        {
            // Arrange
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.ChangeStatus(1, true)).Returns(() => Task.FromResult(new bool()));

            // Act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .Build();

            var view = await sut.ChangeStatus(1, true);

            // Assert
            Assert.NotNull(view);

            var model = view.Data;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(ResponseModel), model);

            sut.AssertPostAttribute("ChangeStatus", new[] { typeof(int), typeof(bool) });
            userService.Verify(x => x.ChangeStatus(1, true), Times.Once);
        }

        [Test]
        public void new_should_return_with_user_model()
        {
            // Act
            var sut = new UserControllerBuilder().Build();

            var view = sut.New() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);

            sut.AssertGetAttribute("New");
            sut.AssertAllowAnonymousAttribute("New");
        }

        [Test]
        public async void new_should_redirect_if_model_is_valid()
        {
            // Arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com", Password = "pass" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value)).Returns(() => Task.FromResult<int?>(1));

            // Act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .Build();
            var view = await sut.New(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value), Times.Once);

            sut.AssertPostAttribute("New", new[] { typeof(UserModel) });
            sut.AssertAllowAnonymousAttribute("New", new[] { typeof(UserModel) });
        }

        [Test]
        public async void new_should_return_with_user_model_if_model_is_invalid()
        {
            // Arrange
            var invalidModel = new UserModel { Name = "test name" };

            // Act
            var sut = new UserControllerBuilder().Build();
            var view = await sut.New(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);

            sut.AssertPostAttribute("New", new[] { typeof(UserModel) });
            sut.AssertAllowAnonymousAttribute("New", new[] { typeof(UserModel) });
        }

        [Test]
        public void reset_should_return_with_reset_model()
        {
            // Act
            var sut = new UserControllerBuilder().BuildWithMockControllerContext();
            var view = sut.Reset() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(ResetModel), model);

            sut.AssertGetAttribute("Reset");
            sut.AssertAllowAnonymousAttribute("Reset");
        }

        [Test]
        public void login_should_return_with_login_model()
        {
            // Act
            var sut = new UserControllerBuilder().Build();
            var view = sut.Login() as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(LoginModel), model);

            sut.AssertGetAttribute("Login");
            sut.AssertAllowAnonymousAttribute("Login");
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
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .WithFormsAuthenticationService(formsAuthenticationService.Object)
                                                 .Build();
            var view = await sut.Login(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            userService.Verify(x => x.Authenticate(validModel.Email, validModel.Password), Times.Once);

            sut.AssertPostAttribute("Login", new[] { typeof(LoginModel) });
            sut.AssertAllowAnonymousAttribute("Login", new[] { typeof(LoginModel) });
        }

        [Test]
        public async void login_should_return_with_login_model_if_model_is_invalid()
        {
            // Arrange
            var invalidModel = new LoginModel { Email = "test@test.com"};   

            // Act
            var sut = new UserControllerBuilder().Build(); ;
            var view = await sut.Login(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            var model = view.Model;
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(LoginModel), model);
 
            sut.AssertPostAttribute("Login", new[] { typeof(LoginModel) });
            sut.AssertAllowAnonymousAttribute("Login", new[] { typeof(LoginModel) });
        }

        [Test]
        public async void logout_should_redirect()
        {
            // Arrange    
            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignOut());

            // Act
            var sut = new UserControllerBuilder().WithFormsAuthenticationService(formsAuthenticationService.Object)
                                                 .Build();

            var view = sut.Logout() as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/");
            sut.AssertGetAttribute("Logout");  
        }

    }
}
