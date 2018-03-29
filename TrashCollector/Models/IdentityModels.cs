using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace TrashCollector.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here => this.ProfileId is a value stored in database against the user
            userIdentity.AddClaim(new Claim("ProfileId", this.ProfileId.ToString()));
            return userIdentity;
        }

        //Extended Properties
        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //This is how you add tables to your context
        //public DbSet<Address> Address { get; set; }
        //public DbSet<State> State { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<TrashCollector.Models.Profile> Profiles { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.Invoice> Invoices { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.Pickup> Pickups { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.TrashCollection> TrashCollections { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.State> States { get; set; }

        public System.Data.Entity.DbSet<TrashCollector.Models.ZipCode> ZipCodes { get; set; }

    }

}