namespace SoftwareTechnologiesTeamProject.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using System.Data.Entity;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Profile> Profile { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Bet> Bets { get; set; }
    }
}