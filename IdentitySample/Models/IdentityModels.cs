using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool isActive { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string IDno { get; set; }
        public string DoB { get; set; }
        public string gender { get; set; }
        public string Address { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<SiteOperator> SiteOperators { get; set; }
        public DbSet<LicenceCode> LicenceCodes { get; set; }
        public DbSet<TruckBrand> TruckBrands { get; set; }
        public DbSet<TruckModel> TruckModels{ get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-one: Company ↔ Account
            modelBuilder.Entity<Company>()
                .HasOptional(c => c.Account)
                .WithRequired(a => a.Company);

            // One-to-one: Account ↔ Administrator
            modelBuilder.Entity<Account>()
                .HasOptional(a => a.Administrator)
                .WithRequired(ad => ad.Account);

            // Many-to-many: Company ↔ Project
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Projects)
                .WithMany(p => p.Companies)
                .Map(cp =>
                {
                    cp.MapLeftKey("CompanyId");
                    cp.MapRightKey("ProjectId");
                    cp.ToTable("CompanyProjects");
                });
        }

    }
}