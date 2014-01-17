using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class RequestLogServiceBuilder
    {
        private IRepository<Token> _tokenRepository;
        private IRepository<RequestLog> _requestLogRepository;

        public RequestLogServiceBuilder()
        {
            _tokenRepository = null;
            _requestLogRepository = null;
        }

        internal RequestLogServiceBuilder WithTokenRepository(IRepository<Token> repo)
        {
            _tokenRepository = repo;
            return this;
        }
        internal RequestLogServiceBuilder WithRequestLogRepository(IRepository<RequestLog> repo)
        {
            _requestLogRepository = repo;
            return this;
        }

        internal RequestLogService Build()
        {
            return new RequestLogService(_tokenRepository, _requestLogRepository);
        }
    }
}
