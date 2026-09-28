using Domain;
using Domain.Common;
using Domain.Framework;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<About> About { get; set; } = null!;
    public DbSet<AboutGallery> AboutGallery { get; set; } = null!;
    public DbSet<Career> Career { get; set; } = null!;
    public DbSet<CareerSection> CareerSection { get; set; } = null!;
    public DbSet<Contact> Contact { get; set; } = null!;
    public DbSet<Department> Department { get; set; } = null!;
    public DbSet<Enquiry> Enquiry { get; set; } = null!;
    public DbSet<Faq> Faq { get; set; } = null!;
    public DbSet<HomeBanner> HomeBanner { get; set; } = null!;
    public DbSet<Insurance> Insurance { get; set; } = null!;
    public DbSet<InsuranceSection> InsuranceSection { get; set; } = null!;
    public DbSet<Loyalty> Loyalty { get; set; } = null!;
    public DbSet<LoyaltyImage> LoyaltyImage { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<NewsImage> NewsImage { get; set; } = null!;
    public DbSet<PageSettings> PageSettings { get; set; } = null!;
    public DbSet<Partner> Partner { get; set; } = null!;
    public DbSet<PartnerCategory> PartnerCategory { get; set; } = null!;
    public DbSet<PartnerSection> PartnerSection { get; set; } = null!;
    public DbSet<Policy> Policy { get; set; } = null!;
    public DbSet<Service> Service { get; set; } = null!;
    public DbSet<ServiceSection> ServiceSection { get; set; } = null!;
    public DbSet<SocialMedia> SocialMedia { get; set; } = null!;
    public DbSet<Team> Team { get; set; } = null!;
    public DbSet<TeamSection> TeamSection { get; set; } = null!;
    public DbSet<TeamContent> TeamContent { get; set; } = null!;
    public DbSet<Testimonial> Testimonial { get; set; } = null!;
    public DbSet<VideoHub> VideoHub { get; set; } = null!;
    public DbSet<Email> Email { get; set; } = null!;    
    public DbSet<CareerContent> CareerContent { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }


    public virtual async Task<int> SaveChangesAsync(string username = "SYSTEM")
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                     .Where(q => q.State is EntityState.Added or EntityState.Modified))
        {
            entry.Entity.ModifiedOn = DateTime.UtcNow;
            entry.Entity.ModifiedBy = username;


            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = DateTime.UtcNow;
                entry.Entity.CreatedBy = username;
            }
        }

        var result = await base.SaveChangesAsync();

        return result;
    }

}