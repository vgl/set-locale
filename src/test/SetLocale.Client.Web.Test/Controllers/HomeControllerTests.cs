using NUnit.Framework;

using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Models;
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

            // Act
            var controller = new HomeController(null, null, null);
            var view = controller.Index();

            // Assert
            Assert.NotNull(view.Result);
            var model = view.Result.Model as HomeStatsModel;
            Assert.NotNull(model);
            controller.AssertGetAttribute("Index");
        }
    }
}