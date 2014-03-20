using Moq;

using set.locale.Controllers;
using set.locale.Data.Services;

namespace set.locale.test.Shared.Builders
{
    public class HomeControllerBuilder : BaseBuilder
    {
        private IFeedbackService _feedbackService;
        private IReportService _reportService;
        private IUserService _userService;

        public HomeControllerBuilder()
        {
            _feedbackService = new Mock<IFeedbackService>().Object;
            _reportService = new Mock<IReportService>().Object;
            _userService = new Mock<IUserService>().Object;
        }

        internal HomeControllerBuilder WithFeedbackService(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            return this;
        }

        internal HomeController Build()
        {
            return new HomeController(_userService, _feedbackService, _reportService);
        }
    }
}