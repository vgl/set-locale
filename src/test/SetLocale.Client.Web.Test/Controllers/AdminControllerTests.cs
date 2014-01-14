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
        [Test]
        public void index_should_return()
        {
            // Act
            var sut = new AdminControllerBuilder().Build();
            var view = sut.Index();

            // Assert
            Assert.NotNull(view);
            sut.AssertGetAttribute("Index");
        }

        [Test]
        public void new_translator_should_return_with_user_model()
        {
            // Act
            var sut = new AdminControllerBuilder().Build();
            var view = sut.NewTranslator();
            var model = view.Model;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(model);
            Assert.IsAssignableFrom(typeof(UserModel), model);

            sut.AssertGetAttribute("NewTranslator");
        }

        [Test]
        public async void new_translator_should_redirect_if_model_is_valid()
        {
            // Arrange
            var validModel = new UserModel { Name = "test name", Email = "test@test.com" };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value)).Returns(() => Task.FromResult<int?>(1));

            // Act
            var sut = new AdminControllerBuilder().WithUserService(userService.Object)
                                                 .Build();
            var view = await sut.NewTranslator(validModel) as RedirectResult;

            // Assert
            Assert.NotNull(view);
            Assert.AreEqual(view.Url, "/admin/users");
            Assert.IsInstanceOf<BaseController>(sut);

            userService.Verify(x => x.Create(It.IsAny<UserModel>(), SetLocaleRole.Translator.Value), Times.Once);
            sut.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
            
        }

 

        [Test]
        public async void new_translator_should_return_with_app_model_if_model_is_invalid()
        {
            // Arrange
            var inValidModel = new UserModel { Name = "test name" };

            // Act
            var sut = new AdminControllerBuilder().Build();
            var view = await sut.NewTranslator(inValidModel) as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            Assert.IsAssignableFrom(typeof(UserModel), view.Model);
            
            sut.AssertPostAttribute("NewTranslator", new[] { typeof(UserModel) });
        }

        //[Test]
        //public async void users_should_return_with_list_user_model()
        //{
        //    // Arrange           
        //    var userService = new Mock<IUserService>();
        //    userService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<User>()));
             
        //    // Act
        //    var sut = new AdminControllerBuilder().WithUserService(userService.Object)
        //                                        .Build();
        //    var view = await sut.Users(5) as ViewResult;     

        //    // Assert
        //    Assert.NotNull(view);
        //    Assert.NotNull(view.Model);
        //    Assert.IsAssignableFrom(typeof(List<UserModel>), view.Model);
        //    sut.AssertGetAttribute("Users", new[] { typeof(int) });
        //    userService.Verify(x => x.GetAll(), Times.Once); 
        //}
         
        //[Test]
        //public async void apps_should_return_with_list_app_model()
        //{
        //    // Arrange           
        //    var appService = new Mock<IAppService>();
        //    appService.Setup(x => x.GetAll()).Returns(() => Task.FromResult(new List<App>()));

        //    // Act
        //    var sut = new AdminControllerBuilder().WithAppService(appService.Object)
        //                                          .Build();

        //    var view = await sut.Apps() as ViewResult;

        //    // Assert
        //    Assert.NotNull(view);
        //    Assert.NotNull(view.Model);
        //    Assert.IsAssignableFrom(typeof(List<AppModel>), view.Model); 
        //    sut.AssertGetAttribute("Apps");
        //    appService.Verify(x => x.GetAll(), Times.Once);
        //}
    }
}
