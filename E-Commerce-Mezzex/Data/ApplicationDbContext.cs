using CloudinaryDotNet.Actions;
using E_Commerce_Mezzex.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }

    // New DbSets
    public DbSet<CustomerReview> CustomerReviews { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public DbSet<RelatedProduct> RelatedProducts { get; set; }
    public DbSet<QuestionAnswer> QuestionsAnswers { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        builder.Entity<UserPermission>()
            .HasKey(up => new { up.UserId, up.PermissionId });

        builder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);

        builder.Entity<UserPermission>()
            .HasOne(up => up.Permission)
            .WithMany(p => p.UserPermissions)
            .HasForeignKey(up => up.PermissionId);

        builder.Entity<Picture>(entity =>
        {
            entity.Property(e => e.MediaType)
                  .HasConversion(
                      v => v.ToString(),
                      v => (MediaType)Enum.Parse(typeof(MediaType), v))
                  .HasMaxLength(50);
        });

        // New entities configurations

        builder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products);

        builder.Entity<Product>()
            .HasOne(p => p.Specifications)
            .WithOne()
            .HasForeignKey<ProductSpecification>(ps => ps.ProductId);

        builder.Entity<Product>()
            .HasMany(p => p.Images)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Resolve multiple cascade paths

        builder.Entity<Product>()
            .HasMany(p => p.CustomerReviews)
            .WithOne()
            .HasForeignKey(cr => cr.ProductId);

        builder.Entity<Product>()
            .HasMany(p => p.RelatedProducts)
            .WithOne(rp => rp.MainProduct)
            .HasForeignKey(rp => rp.MainProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasMany(p => p.QuestionsAnswers)
            .WithOne()
            .HasForeignKey(qa => qa.ProductId);

        builder.Entity<RelatedProduct>()
            .HasOne(rp => rp.MainProduct)
            .WithMany(p => p.RelatedProducts)
            .HasForeignKey(rp => rp.MainProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RelatedProduct>()
            .HasOne(rp => rp.RelatedProductDetails)
            .WithMany()
            .HasForeignKey(rp => rp.RelatedProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ProductVariant relationships

        builder.Entity<Product>()
            .HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProductVariant>()
            .HasMany(v => v.Images)
            .WithOne(i => i.ProductVariant)
            .HasForeignKey(i => i.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProductVariant>()
            .HasOne(v => v.Product)
            .WithMany(p => p.Variants)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
