using System.Data.Entity;
using AlwayEncryptedSample.Models;
using AlwayEncryptedSample.Properties;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AlwayEncryptedSample.Services
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }

        /// <remarks>We override this to set the schema.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Settings.Default.AuthenticationSchema);
            base.OnModelCreating(modelBuilder);
        }
    }
}