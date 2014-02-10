using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Test.Builders
{
    public class TagServiceBuilder
    {
        private IRepository<Word> _wordRepository;
        private IRepository<Tag> _tagRepository;

        public TagServiceBuilder()
        {
            _wordRepository = null;
            _tagRepository = null;
        }

        internal TagServiceBuilder WithWordRepository(IRepository<Word> word, IRepository<Tag> tag)
        {
            _wordRepository = word;
            _tagRepository = tag;
            return this;
        }

        internal TagService Build()
        {
            return new TagService(_wordRepository, _tagRepository);
        }
    }
}
