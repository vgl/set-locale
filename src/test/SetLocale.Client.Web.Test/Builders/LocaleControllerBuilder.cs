using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LocaleControllerBuilder
    {
        private IWordService _wordService;
        private IAppService _appService;
        private IRequestLogService _requestLogService;

        public LocaleControllerBuilder()
        {
            _wordService = null;
            _appService = null;
            _requestLogService = null;
        }

        internal LocaleControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }

        internal LocaleControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;
        }

        internal LocaleControllerBuilder WithRequestLogService(IRequestLogService requestLogService)
        {
            _requestLogService = requestLogService;
            return this;
        }

        internal LocaleController Build()
        {
            return new LocaleController(_wordService, _appService, _requestLogService);
        }
    }
}