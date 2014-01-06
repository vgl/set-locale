using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using MvcContrib.TestHelper;
using NUnit.Framework;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public async void apps_should_return_app_model()
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
            controller.AssertGetAttribute("Apps", new[] { typeof(int) });

            appService.Verify(x => x.GetByUserId(1), Times.Once);

        }

        [Test]
        public async void words_should_return_word_model()
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
        public async void change_status_should_return_response_model()
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
            Assert.IsAssignableFrom(typeof(ResponseModel),model);

            controller.AssertPostAttribute("ChangeStatus", new[] { typeof(int),typeof(bool) }); 
            userService.Verify(x => x.ChangeStatus(1,true), Times.Once);

        }
    }
}
