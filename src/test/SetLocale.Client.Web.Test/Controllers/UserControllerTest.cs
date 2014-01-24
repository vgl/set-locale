using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Controllers;
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
        const string ActionNameWords = "Words";
        const string ActionNameApps = "Apps";

        [Test]
        public async void apps_should_return_with_app_model()
        {
            //arrange
            var list = new List<App> { new App { Id = 1, Tokens = new List<Token>() }, new App { Id = 2, Tokens = new List<Token>() } };
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.GetByUserId(1, 1))
                      .Returns(Task.FromResult(new PagedList<App>(1, 2, 3, list))); 

            //act
            var sut = new UserControllerBuilder().WithAppService(appService.Object)
                                                 .Build();
            var view = await sut.Apps(1, 1) as ViewResult;
            var model = view.Model as PageModel<AppModel>;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsInstanceOf<BaseController>(sut);
            Assert.IsAssignableFrom<PageModel<AppModel>>(view.Model);
            Assert.AreEqual(model.Items.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model.Items);

            appService.Verify(x => x.GetByUserId(1, 1), Times.Once);

            sut.AssertGetAttribute(ActionNameApps, new[] { typeof(int), typeof(int) }); 
        }

        [Test]
        public async void words_should_return_with_word_model()
        {
            //arrange
            var list = new List<Word> { new Word { Id = 1 }, new Word { Id = 2 } };
            var wordService = new Mock<IWordService>();
            wordService.Setup(x => x.GetByUserId(1, 1))
                       .Returns(Task.FromResult(new PagedList<Word>(1, 2, 3, list)));

            //act 
            var sut = new UserControllerBuilder().WithWordService(wordService.Object)
                                                 .Build();

            var view = await sut.Words(1, 1);
            var model = view.Model as PageModel<WordModel>;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsInstanceOf<BaseController>(sut);
            Assert.IsAssignableFrom<PageModel<WordModel>>(view.Model);
            Assert.AreEqual(model.Items.Count, list.Count);
            CollectionAssert.AllItemsAreUnique(model.Items);

            wordService.Verify(x => x.GetByUserId(1, 1), Times.Once);

            sut.AssertGetAttribute(ActionNameWords, new[] { typeof(int), typeof(int) });
        }

        [Test]
        public async void change_status_should_return_with_response_model()
        {
            //arrange
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.ChangeStatus(1, true)).Returns(() => Task.FromResult(new bool()));

            //act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .Build();

            var view = await sut.ChangeStatus(1, true);

            //assert
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
            //act
            var sut = new UserControllerBuilder().Build();

            var view = sut.New() as ViewResult;

            //assert
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
            //arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com", Password = "pass" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value)).Returns(() => Task.FromResult<int?>(1));

            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignIn(string.Format("{0}|{1}|{2}", 1, validModel.Name, validModel.Email), true));

            //act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .WithFormsAuthenticationService(formsAuthenticationService.Object)
                                                 .Build();
            var view = await sut.New(validModel);

            //assert
            Assert.NotNull(view);
            Assert.AreEqual(((RedirectResult)view).Url, "/user/apps");
            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Developer.Value), Times.Once);

            sut.AssertPostAttribute("New", new[] { typeof(UserModel) });
            sut.AssertAllowAnonymousAttribute("New", new[] { typeof(UserModel) });
        }

        [Test]
        public async void new_should_return_with_user_model_if_model_is_invalid()
        {
            //arrange
            var invalidModel = new UserModel { Name = "test name" };

            //act
            var sut = new UserControllerBuilder().Build();
            var view = await sut.New(invalidModel) as ViewResult;

            //assert
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
            //act
            var sut = new UserControllerBuilder().BuildWithMockControllerContext();
            var view = sut.Reset() as ViewResult;

            //assert
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
            //act
            var sut = new UserControllerBuilder().Build();
            var view = sut.Login() as ViewResult;

            //assert
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
            //arrange   
            const int id = 1;
            const string email = "test@test.com";
            const string name = "test";
            var validModel = new LoginModel { Email = email, Password = "pass" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Authenticate(validModel.Email, validModel.Password)).Returns(() => Task.FromResult(true));
            userService.Setup(x => x.GetByEmail(validModel.Email))
                                    .Returns(() => Task.FromResult(new User
                                                                    {
                                                                        Id = id,
                                                                        Name = name,
                                                                        Email = email
                                                                    }));

            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignIn(string.Format("{0}|{1}|{2}", id, name, email), true));

            //act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .WithFormsAuthenticationService(formsAuthenticationService.Object)
                                                 .Build();
            var view = await sut.Login(validModel) as RedirectResult;

            //assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/user/apps");
            userService.Verify(x => x.Authenticate(validModel.Email, validModel.Password), Times.Once);

            sut.AssertPostAttribute("Login", new[] { typeof(LoginModel) });
            sut.AssertAllowAnonymousAttribute("Login", new[] { typeof(LoginModel) });
        }

        [Test]
        public async void login_should_return_with_login_model_if_model_is_invalid()
        {
            //arrange
            var invalidModel = new LoginModel { Email = "test@test.com" };

            //act
            var sut = new UserControllerBuilder().Build(); ;
            var view = await sut.Login(invalidModel) as ViewResult;

            //assert
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
            //arrange    
            var formsAuthenticationService = new Mock<IFormsAuthenticationService>();
            formsAuthenticationService.Setup(x => x.SignOut());

            //act
            var sut = new UserControllerBuilder().WithFormsAuthenticationService(formsAuthenticationService.Object)
                                                 .Build();

            var view = sut.Logout() as RedirectResult;

            //assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/");
            sut.AssertGetAttribute("Logout");
        }

    }
}
