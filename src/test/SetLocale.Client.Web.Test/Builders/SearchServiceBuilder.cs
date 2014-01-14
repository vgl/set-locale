using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class SearchServiceBuilder
    {
        private IRepository<Word> _wordRepository;

        public SearchServiceBuilder()
        {
            _wordRepository = null;
        }

        internal SearchServiceBuilder WithWordRepository(IRepository<Word> repo)
        {
            _wordRepository = repo;
            return this;
        }

        internal SearchService Build()
        {
            return new SearchService(_wordRepository);
        }
    }
}
