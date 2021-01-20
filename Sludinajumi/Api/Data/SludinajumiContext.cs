using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Sludinajumi.Api.Models;
using Sludinajumi.Api.Models.Data;
using Sludinajumi.Api.Controllers;

namespace Sludinajumi.Api.Data
{

    public class SludinajumiContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }

        public DbSet<AdProperty> AdProperties { get; set; }
        public DbSet<Property> Properties { get; set; }

        public DbSet<CategoryProperty> CategoryProperties { get; set; }

        public DbSet<Translation> Translations { get; set; }
        public DbSet<TranslationEntry> TranslationEntries { get; set; }
        public DbSet<Language> Languages { get; set; }

        public SludinajumiContext(DbContextOptions<SludinajumiContext> options)
            :base(options)
        {
        }

        public SludinajumiContext()
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(sc => sc.Parent)
                .HasForeignKey(c => c.ParentId);

            modelBuilder.Entity<Category>()
                .HasIndex(cat => cat.UrlTitle)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(cat => cat.Title)
                .IsUnique();
                        
            modelBuilder.Entity<CategoryProperty>()
                .HasKey(cp => new { cp.CategoryId, cp.PropertyId });

            modelBuilder.Entity<CategoryProperty>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.Properties)
                .HasForeignKey(cp => cp.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CategoryProperty>()
                .HasOne(cp => cp.Property)
                .WithMany(p => p.Categories)
                .HasForeignKey(cp => cp.PropertyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdProperty>()
                .HasKey(ap => new { ap.PropertyId, ap.AdId });

            modelBuilder.Entity<Ad>()
                .HasMany(a => a.ItemProperties)
                .WithOne(ap => ap.Ad)
                .HasForeignKey(ap => ap.AdId);

            modelBuilder.Entity<TranslationEntry>()
                .HasMany(te => te.Translations)
                .WithOne(t => t.TranslationEntry)
                .HasForeignKey(t => t.TranslationEntryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Name)
                .WithMany()
                .HasForeignKey(p => p.NameId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Description)
                .WithMany()
                .HasForeignKey(p => p.DescriptionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }

}