using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class ReportServiceBuilder
    {
        private IRepository<User> _userRepository;
        private IRepository<Word> _wordRepository;
        private IRepository<App> _appRepository;

        public ReportServiceBuilder()
        {
            _userRepository = null;
            _wordRepository = null;
            _appRepository = null;
        }
        internal ReportServiceBuilder WithUserRepository(IRepository<User> repo)
        {
            _userRepository = repo;
            return this;
        }

        internal ReportServiceBuilder WithWordRepository(IRepository<Word> repo)
        {
            _wordRepository = repo;
            return this;
        }

        internal ReportServiceBuilder WithAppRepository(IRepository<App> repo)
        {
            _appRepository = repo;
            return this;
        }

        internal ReportService Build()
        {
            return new ReportService(_userRepository, _wordRepository, _appRepository);
        }
    }
}