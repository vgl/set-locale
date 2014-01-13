using System.Data.Entity;

namespace SetLocale.Client.Web.Repositories
{
    public class SetLocaleDbInitializer : MigrateDatabaseToLatestVersion<SetLocaleDbContext, SetLocaleDbMigrationConfiguration>
    {

    }
}