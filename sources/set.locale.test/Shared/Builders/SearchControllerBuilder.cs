using Moq;

using set.locale.Controllers;
using set.locale.Data.Services;

namespace set.locale.test.Shared.Builders
{
    public class SearchControllerBuilder : BaseBuilder
    {
        private ISearchService _searchService;

        public SearchControllerBuilder()
        {
            _searchService = new Mock<ISearchService>().Object;
        }

        internal SearchControllerBuilder WithSearchService(ISearchService feedbackService)
        {
            _searchService = feedbackService;
            return this;
        }

        internal SearchController Build()
        {
            return new SearchController(_searchService);
        }
    }
}