using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class ReportServiceTests
    {
        [Test]
        public async void should_create_a_homestats()
        {
            //arrange
            var model = new HomeStatsModel
            {
                ApplicationCount = 1,
                DeveloperCount = 2,
                TranslatorCount = 3,
                KeyCount = 4,
                TranslationCount = 5
            };

            //act
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.FindAll(y => y.RoleId != SetLocaleRole.Translator.Value)).Returns(new List<User>
            {
                new User{RoleId = 1},
                new User{RoleId = 2}
            }.AsQueryable());

            userRepository.Setup(x => x.FindAll(y => y.RoleId == SetLocaleRole.Translator.Value)).Returns(new List<User>
            {
                new User{RoleId = 1},
                new User{RoleId = 2},
                new User{RoleId = 3},
            }.AsQueryable());

            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll((It.IsAny<Expression<Func<Word, bool>>>()))).Returns(new List<Word>
            {
                new Word{TranslationCount = 1},
                new Word{TranslationCount = 1},
                new Word{TranslationCount = 1},
                new Word{TranslationCount = 2},
            }.AsQueryable());

            var appRepository = new Mock<IRepository<App>>();
            appRepository.Setup(x => x.FindAll((It.IsAny<Expression<Func<App, bool>>>()))).Returns(new List<App>
            {
                new App{Id = 1}
            }.AsQueryable());

            var service = new ReportService(userRepository.Object, wordRepository.Object, appRepository.Object);
            var homestats = await service.GetHomeStats();

            //assert
            Assert.AreEqual(model.ApplicationCount, homestats.ApplicationCount);
            Assert.AreEqual(model.DeveloperCount, homestats.DeveloperCount);
            Assert.AreEqual(model.TranslatorCount, homestats.TranslatorCount);
            Assert.AreEqual(model.KeyCount, homestats.KeyCount);
            Assert.AreEqual(model.TranslationCount, homestats.TranslationCount);
        }
    }
}
