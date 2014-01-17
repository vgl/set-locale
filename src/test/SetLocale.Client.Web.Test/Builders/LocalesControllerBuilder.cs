using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LocalesControllerBuilder
    {
        private IWordService _wordService;

        public LocalesControllerBuilder()
        {
            _wordService = null;
        }

        internal LocalesControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }
   
        internal LocalesController Build()
        {
            return new LocalesController(_wordService);
        }
    }
}