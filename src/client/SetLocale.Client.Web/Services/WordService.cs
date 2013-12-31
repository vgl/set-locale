using System.Threading.Tasks;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IWordService
    {
        Task<string> Create(KeyModel model);
    }

    public class WordService : IWordService
    {
        private readonly IRepository<Word> _wordRepository;
        public WordService(IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public Task<string> Create(KeyModel model)
        {
            if (!model.IsValidForNew())
            {
                return null;
            }

            var word = new Word
            {
                Key = model.Key,
                Description = model.Description ?? string.Empty,
                IsTranslated = false,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy
            };

            //todo:tags...

            _wordRepository.Create(word);
            _wordRepository.SaveChanges();

            if (word.Id < 1)
            {
                return null;
            }

            return Task.FromResult(word.Key);
        }
    }
}