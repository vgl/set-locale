using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public async void index_should_return_home_stats_model()
        {
            // Arrange
            var reportService = new Mock<IReportService>();
            reportService.Setup(x => x.GetHomeStats()).Returns(() => Task.FromResult(new HomeStatsModel()));

            // Act
            var sut = new HomeControllerBuilder().WithReportService(reportService.Object)
                                                 .Build();

            var view = await sut.Index();

            // Assert
            Assert.NotNull(view);
            var model = view.Model as HomeStatsModel;
            Assert.NotNull(model);
            sut.AssertGetAttribute("Index");
        }
    }
}