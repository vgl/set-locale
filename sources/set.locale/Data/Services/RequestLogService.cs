using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using set.locale.Data.Entities;

namespace set.locale.Data.Services
{
    public interface IRequestLogService
    {
        Task<bool> Log(string token, string ip, string url);
    }

    public class RequestLogService : BaseService, IRequestLogService
    {
        public Task<bool> Log(string token, string ip, string url)
        {
            if (string.IsNullOrEmpty(token)
                && string.IsNullOrEmpty(url)) return Task.FromResult(false);

            var tokenEntity = Context.Tokens.FirstOrDefault(x => x.Key == token);
            if (tokenEntity == null) return Task.FromResult(false);

            tokenEntity.UsageCount = tokenEntity.UsageCount + 1;

            var log = new RequestLog
            {
                Token = token,
                IP = ip,
                Url = url
            };
            Context.RequestLogs.Add(log);
            Context.Entry(log).State = EntityState.Added;
            return Task.FromResult(Context.SaveChanges() > 0);
        }
    }
}