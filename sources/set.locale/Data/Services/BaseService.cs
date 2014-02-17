namespace set.locale.Data.Services
{
    public class BaseService
    {
        public readonly SetDbContext Context;

        public BaseService(SetDbContext context = null)
        {
            if (context == null)
            {
                context = new SetDbContext();
            }

            Context = context;
        }
    }
}