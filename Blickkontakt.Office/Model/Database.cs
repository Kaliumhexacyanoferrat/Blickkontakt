using Microsoft.EntityFrameworkCore;

namespace Blickkontakt.Office.Model
{

    public class Database : DbContext
    {
        private static DbContextOptions<Database>? _Options;

        #region Factory

        public static string ConnectionString
        {
            get
            {
                return "Server=localhost;Database=blickkontakt;User Id=blickkontakt;Password=blickkontakt";
            }
        }

        public static Database Create()
        {
            return new Database(_Options ??= GetOptions(true));
        }

        private static DbContextOptions<Database> GetOptions(bool tracking)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Database>();

            optionsBuilder.UseNpgsql(ConnectionString);

            return optionsBuilder.Options;
        }

#pragma warning disable CS8618

        private Database(DbContextOptions options) : base(options) { }

#pragma warning restore CS8618

        #endregion

        #region Entities

        public DbSet<Account> Users { get; set; }

        #endregion


    }

}
