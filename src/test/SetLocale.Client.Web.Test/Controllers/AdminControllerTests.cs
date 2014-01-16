using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.Builders;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AdminControllerTests
    {
        const string ActionNameApps = "Apps";
        const string ActionNameUsers = "Users";

        [Test]
        public void index_should_return()
        {
            //act
            var sut = new AdminControllerBuilder().Build();
            var view = sut.Index();

            //assert
            Assert.NotNull(view);
            sut.AssertGetAttribute("Index");
        }

        [Test]
        public void new_translator_should_return_with_user_model()
        {
            //act
            var sut = new AdminControllerBuilder().Build();
            var view = sut.NewTranslator();
            var model = view.Model;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);

            sut.AssertGetAttribute("NewTranslator");
        }

        [Test]
        public async void new_translator_should_redirect_if_model_is_valid()
        {
            //arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value)).Returns(() => Task.FromResult<int?>(1));

            //act
            var sut = new AdminControllerBuilder().WithUserService(userService.Object)
                                                 .Build();
            var view = await sut.NewTranslator(validModel) as RedirectResult;

            //assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/admin/users");
            Assert.IsInstanceOf<BaseController>(sut);

            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value), Times.Once);
            sut.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });

        }

        [Test]
        public async void new_translator_should_return_with_app_model_if_model_is_invalid()
        {
            //arrange
            var inValidModel = new UserModel { Name = "test name" };

            //act
            var sut = new AdminControllerBuilder().Build();
            var view = await sut.NewTranslator(inValidModel) as ViewResult;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(UserModel), view.Model);

            sut.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
        }

        [Test]
        public async void users_should_return_with_paged_user_model_if_user_id_is_invalid()
        {
            //arrange           
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetUsers(1)).Returns(() => Task.FromResult(new PagedList<User>(1, 1, 1, new List<User>())));

            //act
            var sut = new AdminControllerBuilder().WithUserService(userService.Object)
                                                .Build();
            var view = await sut.Users(0, 1) as ViewResult;  // id = 0 => invalid UserId 

            //assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(PageModel<UserModel>), view.Model);

            sut.AssertGetAttribute(ActionNameUsers, new[] { typeof(int), typeof(int) });
            userService.Verify(x => x.GetUsers(1), Times.Once);
        }

        [Test]
        public async void users_should_return_with_paged_user_model_if_user_id_is_valid()
        {
            //arrange           
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetAllByRoleId(SetLocaleRole.Admin.Value, 1)).Returns(() => Task.FromResult(new PagedList<User>(1, 1, 1, new List<User>())));

            //act
            var sut = new AdminControllerBuilder().WithUserService(userService.Object)
                                                .Build();
            var view = await sut.Users(SetLocaleRole.Admin.Value, 1) as ViewResult;  // id = 0 => invalid UserId 

            //assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(PageModel<UserModel>), view.Model);

            sut.AssertGetAttribute(ActionNameUsers, new[] { typeof(int), typeof(int) });
            userService.Verify(x => x.GetAllByRoleId(SetLocaleRole.Admin.Value, 1), Times.Once);
        }

        [Test]
        public async void apps_should_return_with_list_app_model()
        {
            //arrange           
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.GetApps(1)).Returns(Task.FromResult(new PagedList<App>(1, 1, 1, new List<App>())));

            //act
            var sut = new AdminControllerBuilder().WithAppService(appService.Object)
                                                  .Build(); 
            var view = await sut.Apps(1) as ViewResult;

            //assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(PageModel<AppModel>), view.Model);

            sut.AssertGetAttribute(ActionNameApps, new[] { typeof(int) });
            appService.Verify(x => x.GetApps(1), Times.Once);
        }
    }
}
