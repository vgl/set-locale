using Moq;

using set.locale.Controllers;
using set.locale.Data.Services;

namespace set.locale.test.Shared.Builders
{
    public class HomeControllerBuilder : BaseBuilder
    {
        private IFeedbackService _feedbackService;

        public HomeControllerBuilder()
        {
            _feedbackService = new Mock<IFeedbackService>().Object;
        }

        internal HomeControllerBuilder WithFeedbackService(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            return this;
        }

        internal HomeController Build()
        {
            return new HomeController(_feedbackService);
        }
    }
}