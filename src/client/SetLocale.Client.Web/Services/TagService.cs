using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface ITagService
    {
        Task<List<Word>> GetWords(string tagUrlName);
    }

    public class TagService : ITagService
    {
        private readonly IRepository<Word> _wordRepository; 

        public TagService(
            IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public Task<List<Word>> GetWords(string tagUrlName)
        {
            var words = _wordRepository.FindAll(x => x.Tags.Any(y => y.UrlName == tagUrlName)).ToList();
            return Task.FromResult(words);
        }


    }
}