using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LocalesControllerBuilder
    {
        private IWordService _wordService;
        private ITagService _tagService;

        public LocalesControllerBuilder()
        {
            _wordService = null;
            _tagService = null;
        }

        internal LocalesControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }

        internal LocalesControllerBuilder WithTagService(ITagService tagService)
        {
            _tagService = tagService;
            return this;
        }

        internal LocalesController Build()
        {
            return new LocalesController(_wordService, _tagService);
        }
    }
}