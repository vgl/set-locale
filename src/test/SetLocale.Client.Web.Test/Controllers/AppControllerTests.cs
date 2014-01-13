using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Moq;
using MvcContrib.TestHelper.Fakes;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Test.TestHelpers;
using SetLocale.Client.Web.Test.Builders;


namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    class AppControllerTests
    {
        [Test]
        public async void detail_id_is_greater_than_zero_should_return_app_model()
        {
            // Arrange           
            var appService = new Mock<IAppService>();
            appService.Setup(x => x.Get(1)).Returns(() => Task.FromResult(new App{ Id=1, Tokens = new List<Token>(), Url = "url"} ));

            // Act
            var sut = new AppControllerBuilder().WithAppService(appService.Object)
                                                .Build();

            var view = await sut.Detail(1) as ViewResult;

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);

            sut.AssertGetAttribute("Detail", new []{ typeof(int)});
            appService.Verify(x => x.Get(1), Times.Once);
        }

        [Test]
        public async void detail_id_is_lesser_than_one_should_redirect_to_home_index()
        {
            // Arrange           
            var appService = new Mock<IAppService>();   

            // Act 
            var sut = new AppControllerBuilder().WithAppService(appService.Object)
                                                .Build();

            var view = await sut.Detail(0) as RedirectResult;

            // Assert
            Assert.NotNull(view); 
            Assert.AreEqual(view.Url, "/home/index"); 
            sut.AssertGetAttribute("Detail", new[] { typeof(int) });              
        }
        
        [Test]
        public void new_should_return_app_model()
        {  
            // Act
            var sut = new AppControllerBuilder().Build();

            var view = sut.New();

            // Assert
            Assert.NotNull(view);
            sut.AssertGetAttribute("New"); 
        }
          
        [Test]
        public void new_should_return_app_model_if_model_is_invalid()
        {
            // Arrange
            var appService = new Mock<IAppService>();
            var inValidModel = new AppModel { Name = "test name", Url = "test.com" };

            // Act
            
            var sut = new AppControllerBuilder().WithAppService(appService.Object)
                                                  .Build();

            var view = sut.New(inValidModel).Result as ViewResult; 

            // Assert
            Assert.NotNull(view);
            Assert.NotNull(view.Model);
            var model = view.Model as AppModel;

            Assert.NotNull(model);

            sut.AssertPostAttribute("New", new[] { typeof(AppModel) });
        }
    }
     
}
