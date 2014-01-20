using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IRequestLogService
    {
        /// <summary>
        /// logs the api requests
        /// </summary>
        /// <param name="token">the request token passed in http header</param>
        /// <param name="ip">the ip request made</param>
        /// <param name="url">the url of the request</param>
        /// <returns></returns>
        Task<bool> Log(string token, string ip, string url);
    }

    public class RequestLogService : IRequestLogService
    {
        private readonly IRepository<Token> _tokenRepository;
        private readonly IRepository<RequestLog> _requestLogRepository;

        public RequestLogService(
            IRepository<Token> tokenRepository, 
            IRepository<RequestLog> requestLogRepository)
        {
            _tokenRepository = tokenRepository;
            _requestLogRepository = requestLogRepository;
        }

        public Task<bool> Log(string token, string ip, string url)
        {
            if (string.IsNullOrEmpty(token)
                && string.IsNullOrEmpty(url)) return Task.FromResult(false);

            var tokenEntity = _tokenRepository.FindOne(x => x.Key == token);
            if (tokenEntity == null) return Task.FromResult(false);

            tokenEntity.UsageCount = tokenEntity.UsageCount + 1;

            var log = new RequestLog
            {
                Token = token,
                IP = ip,
                Url = url
            };
            _requestLogRepository.Create(log);
            _tokenRepository.Update(tokenEntity);
            _tokenRepository.SaveChanges();

            return Task.FromResult(_requestLogRepository.SaveChanges());
        }
    }
}