using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class WordServiceBuilder
    {
        private IRepository<Word> _wordRepository;

        public WordServiceBuilder()
        {
            _wordRepository = null;
        }

        internal WordServiceBuilder WithWordRepository(IRepository<Word> repo)
        {
            _wordRepository = repo;
            return this;
        }

        internal WordService Build()
        {
            return new WordService(_wordRepository);
        }
    }
}
