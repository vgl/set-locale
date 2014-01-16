using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LocaleControllerBuilder
    {
        private IWordService _wordService;

        public LocaleControllerBuilder()
        {
            _wordService = null; 
        }

        internal LocaleControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }
         
        internal LocaleController Build()
        {
            return new LocaleController(_wordService);
        }
    }
}