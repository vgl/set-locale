using SetLocale.Client.Web.ApiControllers;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class LocalesControllerBuilder
    {
        private IWordService _wordService;
        private IAppService _appService;
        private IRequestLogService _requestLogService;

        public LocalesControllerBuilder()
        {
            _wordService = null;
            _appService = null;
            _requestLogService = null;
        }

        internal LocalesControllerBuilder WithWordService(IWordService wordService)
        {
            _wordService = wordService;
            return this;
        }
        internal LocalesControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;
        }

        internal LocalesControllerBuilder WithRequestLogService(IRequestLogService requestLogService)
        {
            _requestLogService = requestLogService;
            return this;
        }
        internal LocalesController Build()
        {
            return new LocalesController(_wordService, _appService, _requestLogService);
        }
    }
}