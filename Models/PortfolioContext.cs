using Microsoft.EntityFrameworkCore;
using Portfolio.Models.Portfolio;

namespace Portfolio.Models
{
    public class PortfolioContext : DbContext
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<HomePage> HomePages { get; set; } = null!;
        public DbSet<Video> Videos { get; set; } = null!;
        public DbSet<SkillsCategory> SkillsCategories { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<About> Abouts { get; set; } = null!;
        public DbSet<HeroTemplate> HeroTemplates { get; set; } = null!;
        public DbSet<FeaturesSection> FeaturesSections { get; set; } = null!;
        public DbSet<FeaturesTemplate> FeaturesTemplates { get; set; } = null!;
        public DbSet<CTASection> CTASections { get; set; } = null!;
        public DbSet<CTATemplate> CTATemplates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Admin entity
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure HomePage entity
            modelBuilder.Entity<HomePage>(entity =>
            {
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.DisplayOrder);
            });

            // Configure SkillsCategory entity
            modelBuilder.Entity<SkillsCategory>(entity =>
            {
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.DisplayOrder);
            });
        }
    }
}