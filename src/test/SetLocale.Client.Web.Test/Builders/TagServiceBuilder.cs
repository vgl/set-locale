using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class TagServiceBuilder
    {
        private IRepository<Word> _wordRepository;

        public TagServiceBuilder()
        {
            _wordRepository = null;
        }

        internal TagServiceBuilder WithWordRepository(IRepository<Word> repo)
        {
            _wordRepository = repo;
            return this;
        }

        internal TagService Build()
        {
            return new TagService(_wordRepository);
        }
    }
}
