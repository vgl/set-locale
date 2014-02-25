using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using set.locale.Helpers;

namespace set.locale.Data.Services
{
    public class MsgService : IMsgService
    {
        public void SendEMail(string email, string subject, string htmlBody)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ConstHelper.MediaTypeJson));
                string appSetting = ConfigurationManager.AppSettings[ConstHelper.MessagingApiKey];
                if (string.IsNullOrEmpty(appSetting)) return;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appSetting);
                client.BaseAddress = new Uri("http://msg.setcrm.com/");

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("to", email),
                    new KeyValuePair<string, string>("subject", subject),
                    new KeyValuePair<string, string>("htmlBody", htmlBody)
                });
                var result = client.PostAsync("/Api/SendEmail", content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
            }
        }
    }

    public interface IMsgService
    {
        void SendEMail(string email, string subject, string htmlBody);
    }
}