using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class AppServiceTests
    {
        [Test]
        public void should_create_return_with_app_id_if_model_is_valid()
        {
            // Arrange
            var appModel = new AppModel { Name = "test", Url = "test.com", Description = "test_desc" };

            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.Create(It.IsAny<App>())).Returns(new App { Id = 1 });
            appRepository.Setup(x => x.SaveChanges()).Returns(true);

            // Act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var appId = sut.Create(appModel);     //todo:Create olayýndan sonra id = 0 geldiði için null geri dönüyor kontrol et.

            // Assert
            Assert.NotNull(appId);
            Assert.IsAssignableFrom<int>(appId); 

            appRepository.Verify(x => x.Create(It.IsAny<App>()), Times.Once);
            appRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce);

        }

        [Test]
        public async void should_create_return_null_if_model_is_invalid()
        {
            // Arrange
            var invalidModel = new AppModel { Name = "invalidApp" };
             
            // Act
            var sut = new AppServiceBuilder().Build();
            var appId = await sut.Create(invalidModel);

            // Assert
            Assert.Null(appId);  
        }

        [Test]
        public async void should_get_apps_return_with_paged_list_app()
        {
            // Arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(It.IsAny<App>);
             
            // Act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var list = await sut.GetApps(1);

            // Assert
            Assert.NotNull(list);
            Assert.IsAssignableFrom<PagedList<App>>(list);

            appRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_get_by_user_id_return_with_paged_list_app()
        {
            // Arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(It.IsAny<App>);

            // Act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var list = await sut.GetByUserId(1,1);

            // Assert
            Assert.NotNull(list);
            Assert.IsAssignableFrom<PagedList<App>>(list);

            appRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        /*
         * GetByUserEmail Herhangi bir yerde kullanýlmýyor sanýrým kullanýlmayacak da. Silinebilir ?
         * Bu sebeple testini yapmadým.
         */

        [Test]
        public async void should_get_return_with_app()
        {
            // Arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(new App());    // todo:App içine Token vererek dene.

            // Act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var entity = await sut.Get(1);

            // Assert
            Assert.NotNull(entity);             // Null dönme ihtimali var mý ?
            Assert.IsAssignableFrom<App>(entity);

            appRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>()), Times.Once); 
        }

        [Test]
        public async void should_change_status_return_with_true_if_app_is_exist()
        {
            // Arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(new App());

            // Act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var result = await sut.ChangeStatus(1,true);

            // Assert
            Assert.IsTrue(result); 

            appRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_change_status_return_with_false_if_app_is_not_exist()
        {
            // Arrange  
            // Act
            var sut = new AppServiceBuilder().Build();
            var result = await sut.ChangeStatus(0, true);

            // Assert
            Assert.IsFalse(result); 
        }

    }
}