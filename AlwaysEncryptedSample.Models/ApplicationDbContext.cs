using System.Data.Common;
using System.Data.Entity;
using AlwaysEncryptedSample.Models.Properties;

namespace AlwaysEncryptedSample.Models
{
    public class ApplicationDbContext : DbContext
    {
        /// <remarks>Use DefaultConnection so we use one connection string for this and the ASP.NET authentication.</remarks>
        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <remarks>Constructor for unit testing.</remarks>
        internal ApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public static ApplicationDbContext Create(string nameOrConnectionString = "DefaultConnection")
        {
            return new ApplicationDbContext(nameOrConnectionString);
        }

        /// <remarks>We override this to set the schema.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Settings.Default.CreditCardSchema);
            base.OnModelCreating(modelBuilder);
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<CreditCardNetwork> CreditCardNetworks { get; set; }
    }
}