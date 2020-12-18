using System;

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
                var server = Environment.GetEnvironmentVariable("BLICKKONTAKT_DB_HOST") ?? "localhost";
                var db = Environment.GetEnvironmentVariable("BLICKKONTAKT_DB_DATABASE") ?? "blickkontakt";
                var user = Environment.GetEnvironmentVariable("BLICKKONTAKT_DB_USER") ?? "blickkontakt";
                var password = Environment.GetEnvironmentVariable("BLICKKONTAKT_DB_PASSWORD") ?? "blickkontakt";

                return $"Server={server};Database={db};User Id={user};Password={password}";
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

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Announce> Announces { get; set; }

        public DbSet<Letter> Letters { get; set; }

        public DbSet<LetterAnnounce> LetterAnnounces { get; set; }

        #endregion


    }

}
