using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class AppServiceBuilder
    { 
        private IRepository<Token> _tokenRepository;  
        private IRepository<App> _appRepository;

        public AppServiceBuilder()
        {
            _tokenRepository = null;
            _appRepository = null;
        }

        internal AppServiceBuilder WithTokenRepository(IRepository<Token> repo)
        {
            _tokenRepository = repo;
            return this;
        } 

        internal AppServiceBuilder WithAppRepository(IRepository<App> repo)
        {
            _appRepository = repo;
            return this;
        }

        internal AppService Build()
        {
            return new AppService(_tokenRepository, _appRepository);
        }
    }
}