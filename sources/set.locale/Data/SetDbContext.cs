using System.Data.Entity;

using set.locale.Data.Entities;

namespace set.locale.Data
{
    public class SetDbContext : DbContext
    {
        public SetDbContext(string connectionStringOrName)
            : base(connectionStringOrName)
        {
            Database.SetInitializer(new SetDbInitializer());
        }

        public SetDbContext()
            : this("Name=SetWeb")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<DomainObject> DomainObjects { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
