namespace set.locale.Data.Services
{
    public class MsgService : IMsgService
    {
        public void SendEMail(string email, string subject, string htmlBody)
        {
            //todo:implement your mail sending logic
        }
    }

    public interface IMsgService
    {
        void SendEMail(string email, string subject, string htmlBody);
    }
}