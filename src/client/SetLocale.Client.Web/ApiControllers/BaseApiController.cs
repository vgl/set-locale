using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Castle.Core.Internal;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.ApiControllers
{
    public class BaseApiController : ApiController
    {
        private readonly IAppService _appService;
        private readonly IRequestLogService _requestLogService;

        public BaseApiController(IAppService appService, IRequestLogService requestLogService)
        {
            _appService = appService;
            _requestLogService = requestLogService;
        }

        public async Task<bool> IsTokenCheckedAndLoged()
        {
            var token = GetTokenFromRequestHeader();
            if (token.IsNullOrEmpty()) return false;

            var ip = GetClientIp(Request);
            var url = Url.Request.RequestUri.OriginalString;
            
            var isTokenValid = await _appService.IsTokenValid(token);
            if (!isTokenValid) return false;

            var isRequestLogged = await _requestLogService.Log(token, ip, url);
            
            return isRequestLogged;
        }

        //todo: check if this method is right
        private string GetTokenFromRequestHeader()
        {
            var authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
            
            return authHeader.Value.FirstOrDefault(x => x.Contains("Token"));
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            return request.Properties.ContainsKey("MS_HttpContext") ? ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress : null;
        }
    }
}