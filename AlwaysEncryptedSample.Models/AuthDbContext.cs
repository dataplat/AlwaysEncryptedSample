using System.Data.Entity;
using AlwaysEncryptedSample.Models.Properties;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AlwaysEncryptedSample.Models
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, throwIfV1Schema: true)
        {
        }

        public static AuthDbContext Create()
        {
            return Create("name=DefaultConnection");
        }

        public static AuthDbContext Create(string nameOrConnectionString)
        {
            return new AuthDbContext(nameOrConnectionString);
        }

        /// <remarks>We override this to set the schema.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Settings.Default.AuthenticationSchema);
            base.OnModelCreating(modelBuilder);
        }
    }
}