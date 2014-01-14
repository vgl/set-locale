using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private readonly IRepository<App> _appRepository;

        public ReportService(
            IRepository<User> userRepository,
            IRepository<Word> wordRepository,
            IRepository<App> appRepository)
        {
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _appRepository = appRepository;
        }

        public Task<HomeStatsModel> GetHomeStats()
        {
            var developerCount = _userRepository.FindAll(x => x.RoleId != SetLocaleRole.Developer.Value).Count();
            var translatorCount = _userRepository.FindAll(x => x.RoleId == SetLocaleRole.Translator.Value).Count();
            var keyCount = _wordRepository.FindAll().Count();
            var appCount = _appRepository.FindAll().Count();
            var translationCount = _wordRepository.FindAll().Sum(x => x.TranslationCount);

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