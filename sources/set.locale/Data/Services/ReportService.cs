using System.Linq;
using System.Threading.Tasks;
using set.locale.Data.Entities;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public interface IReportService
    {
        Task<HomeStatsModel> GetHomeStats();
    }
    public class ReportService : BaseService, IReportService
    {
        public Task<HomeStatsModel> GetHomeStats()
        {
            var developerCount = Context.Users.Count(x => x.RoleId != SetLocaleRole.Developer.Value);
            var translatorCount = Context.Users.Count(x => x.RoleId == SetLocaleRole.Translator.Value);
            var keyCount = Context.Words.Count();
            var appCount = Context.Apps.Count();
            var translationCount = Context.Words.Sum(x => x.TranslationCount);

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