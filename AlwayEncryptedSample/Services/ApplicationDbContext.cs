using AlwayEncryptedSample.Models;
using AlwayEncryptedSample.Properties;

namespace AlwayEncryptedSample.Services
{
    using System.Data.Entity;
 
    public class ApplicationDbContext : DbContext
    {
        /// <remarks>Use DefaultConnection so we use one connection string for this and the ASP.NET authnetication.</remarks>
        public ApplicationDbContext()
            : base("name=DefaultConnection")
        {
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