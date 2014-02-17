using set.locale.Controllers;
using set.locale.Data.Services;

namespace set.locale.test.Shared.Builders
{
    public class FeedbackControllerBuilder : BaseBuilder
    {
        private IFeedbackService _feedbackService;

        public FeedbackControllerBuilder()
        {
            _feedbackService = null;
        }

        internal FeedbackControllerBuilder WithFeedbackService(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            return this;
        }

        internal FeedbackController BuildWithMockControllerContext(string id, string name, string email, string role)
        {
            var sut = Build();

            SetCurrentUser(id, name, email, role);

            sut.ControllerContext = ControllerContext.Object;
            return sut;
        }

        internal FeedbackController Build()
        {
            return new FeedbackController(_feedbackService);
        }
    }
}