using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Test.TestHelpers;

namespace SetLocale.Client.Web.Test.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void index_should_return_home_stats_model()
        {
            // Arrange
            var reportService = new Mock<IReportService>();
            reportService.Setup(x => x.GetHomeStats()).Returns(() => Task.FromResult(new HomeStatsModel()));

            // Act
            var controller = new HomeController(reportService.Object, null, null);
            var view = controller.Index();

            // Assert
            Assert.NotNull(view.Result);
            var model = view.Result.Model as HomeStatsModel;
            Assert.NotNull(model);
            controller.AssertGetAttribute("Index");
        }
    }
}