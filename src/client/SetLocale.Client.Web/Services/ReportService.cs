using System.Linq;
using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IReportService
    {
        Task<HomeStatsModel> GetHomeStats();
    }
    public class ReportService : IReportService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Word> _wordRepository;

        public ReportService(
            IRepository<User> userRepository,
            IRepository<Word> wordRepository)
        {
            _userRepository = userRepository;
            _wordRepository = wordRepository;
        }

        public Task<HomeStatsModel> GetHomeStats()
        {
            var developerCount = _userRepository.FindAll(x => x.RoleId != SetLocaleRole.Translator.Value).Count();
            var translatorCount = _userRepository.FindAll(x => x.RoleId == SetLocaleRole.Translator.Value).Count();
            var keyCount = _wordRepository.FindAll().Count();
            var appCount = 0;
            var translationCount = 0;

            var model = new HomeStatsModel
            {
                ApplicationCount = appCount,
                DeveloperCount = developerCount,
                TranslatorCount = translatorCount,
                KeyCount = keyCount,
                TranslationCount = translationCount
            };

            return Task.FromResult(model);
        }
    }
}