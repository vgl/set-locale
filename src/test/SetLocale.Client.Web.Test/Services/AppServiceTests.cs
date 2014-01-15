using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
        public async void should_create_return_with_app_id_if_model_is_valid()
        {
            //arrange
            var appModel = new AppModel { Name = "test", Url = "test.com", Description = "test_desc" };

            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.Create(It.IsAny<App>())).Returns(new App());
            appRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var appId = await sut.Create(appModel);

           //assert
            Assert.NotNull(appId);
            Assert.IsAssignableFrom<int>(appId);

            appRepository.Verify(x => x.Create(It.IsAny<App>()), Times.Once);
            appRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }

        [Test]
        public async void should_create_return_null_if_model_is_invalid()
        {
            //arrange
            var invalidModel = new AppModel { Name = "invalidApp" };

            //act
            var sut = new AppServiceBuilder().Build();
            var task = await sut.Create(invalidModel);

           //assert
            Assert.Null(task);
        }

        [Test]
        public async void should_get_apps_return_with_paged_list_app()
        {
            //arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(It.IsAny<App>);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var list = await sut.GetApps(1);

           //assert
            Assert.NotNull(list);
            Assert.IsAssignableFrom<PagedList<App>>(list);

            appRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_get_by_user_id_return_with_paged_list_app()
        {
            //arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(It.IsAny<App>);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var list = await sut.GetByUserId(1, 1);

           //assert
            Assert.NotNull(list);
            Assert.IsAssignableFrom<PagedList<App>>(list);

            appRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_get_return_with_app()
        {
            //arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>(), It.IsAny<Expression<Func<App, object>>>())).Returns(new App());

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var entity = await sut.Get(1);

           //assert
            Assert.NotNull(entity);
            Assert.IsAssignableFrom<App>(entity);

            appRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>(), It.IsAny<Expression<Func<App, object>>>()), Times.Once);
        }

        [Test]
        public async void should_create_token_return_false_if_token_model_is_invalid()
        {
            //arrange 
            var invalidModel = new TokenModel();

            //act
            var sut = new AppServiceBuilder().Build();
            var result = await sut.CreateToken(invalidModel);

           //assert
            Assert.IsFalse(result);
            Assert.IsInstanceOf<IAppService>(sut);
        }

        [Test]
        public async void should_create_token_return_false_if_app_is_not_exist()
        {
            //arrange 
            var validModel = new TokenModel { AppId = 1, Token = "token" };

            var appRepository = new Mock<IRepository<App>>();
            var tokenRepository = new Mock<IRepository<Token>>();

            appRepository.Setup(x => x.Any(It.IsAny<Expression<Func<App, bool>>>())).Returns(false);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .WithTokenRepository(tokenRepository.Object)
                                             .Build();
            var result = await sut.CreateToken(validModel);

           //assert
            Assert.IsFalse(result);
            Assert.IsAssignableFrom<bool>(result);
            Assert.IsInstanceOf<IAppService>(sut);

            appRepository.Verify(x => x.Any(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
            tokenRepository.Verify(x => x.Create(It.IsAny<Token>()), Times.Never);
            tokenRepository.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Test]
        public async void should_create_token_return_true_if_app_is_exist()
        {
            //arrange 
            var validModel = new TokenModel { AppId = 1, Token = "token" };

            var appRepository = new Mock<IRepository<App>>();
            var tokenRepository = new Mock<IRepository<Token>>();

            appRepository.Setup(x => x.Any(It.IsAny<Expression<Func<App, bool>>>())).Returns(true);

            tokenRepository.Setup(x => x.Create(It.IsAny<Token>())).Returns(new Token { Id = 1 });
            tokenRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .WithTokenRepository(tokenRepository.Object)
                                             .Build();
            var result = await sut.CreateToken(validModel);

           //assert
            Assert.IsTrue(result);
            Assert.IsAssignableFrom<bool>(result);
            Assert.IsInstanceOf<IAppService>(sut);

            appRepository.Verify(x => x.Any(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
            tokenRepository.Verify(x => x.Create(It.IsAny<Token>()), Times.Once);
            tokenRepository.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }

        [Test]
        public async void should_create_token_return_true_if_token_model_is_valid()
        {
            //arrange 
            var validModel = new TokenModel { AppId = 1, Token = "token" };
            var appEntity = new App { Id = 1, Name = "testApp", Url = "test.com", UserEmail = "test@test.com" };

            var appRepository = new Mock<IRepository<App>>();
            var tokenRepository = new Mock<IRepository<Token>>();

            appRepository.Setup(x => x.Any(It.IsAny<Expression<Func<App, bool>>>())).Returns(true);

            tokenRepository.Setup(x => x.Create(It.IsAny<Token>())).Returns(new Token { Id = 1 });
            tokenRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .WithTokenRepository(tokenRepository.Object)
                                             .Build();
            var result = await sut.CreateToken(validModel);

           //assert
            Assert.IsTrue(result);
            Assert.IsInstanceOf<IAppService>(sut);
        }

        [Test]
        public async void should_change_status_return_with_true_if_app_is_exist()
        {
            //arrange 
            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>())).Returns(new App());

            //act
            var sut = new AppServiceBuilder().WithAppRepository(appRepository.Object)
                                             .Build();
            var result = await sut.ChangeStatus(1, true);

           //assert
            Assert.IsTrue(result);

            appRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<App, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_change_status_return_with_false_if_app_is_not_exist()
        {
            //arrange  
            //act
            var sut = new AppServiceBuilder().Build();
            var result = await sut.ChangeStatus(0, true);

           //assert
            Assert.IsFalse(result);
        }

    }
}