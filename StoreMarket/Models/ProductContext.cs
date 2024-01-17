using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace StoreMarket.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<Product> Products { get; private set; }
        public DbSet<Category> Categories { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = this.GetService<IConfiguration>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DB")).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(x => x.Id).HasName("ProductID");
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Price).HasColumnName("Price").IsRequired();
                entity.HasOne(x => x.Category).WithMany(c => c.Products).HasForeignKey(x => x.Id).HasConstraintName("GroupToProduct");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ProductsCategory");
                entity.HasKey(x => x.Id).HasName("CategoryID");
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storage");
                entity.HasKey(x => x.Id).HasName("StorageID");
                entity.Property(e => e.Name).HasColumnName("StorageName");
                entity.Property(e => e.Count).HasColumnName("StorageCount");
                entity.HasMany(x => x.Products)
                .WithMany(m => m.Storages)
                .UsingEntity(j => j.ToTable("StorageProduct"));
            });
        }
    }
}
