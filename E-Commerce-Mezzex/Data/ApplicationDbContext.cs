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

    public DbSet<CustomerReview> CustomerReviews { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public DbSet<RelatedProduct> RelatedProducts { get; set; }
    public DbSet<QuestionAnswer> QuestionsAnswers { get; set; }
    public DbSet<VariationType> VariationTypes { get; set; }
    public DbSet<VariationValue> VariationValues { get; set; }
    public DbSet<ProductVariationValue> ProductVariationValues { get; set; }

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

        // Configure VariationType and VariationValue
        builder.Entity<VariationType>()
            .HasMany(vt => vt.VariationValues)
            .WithOne(vv => vv.VariationType)
            .HasForeignKey(vv => vv.VariationTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
            .HasMany(p => p.VariationTypes)
            .WithMany(vt => vt.Products);

        // Configure the many-to-many relationship between Product and VariationValue
        builder.Entity<ProductVariationValue>()
            .HasKey(pvv => new { pvv.ProductId, pvv.VariationValueId });

        builder.Entity<ProductVariationValue>()
            .HasOne(pvv => pvv.Product)
            .WithMany(p => p.ProductVariationValues)
            .HasForeignKey(pvv => pvv.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Specify RESTRICT to avoid cycles

        builder.Entity<ProductVariationValue>()
            .HasOne(pvv => pvv.VariationValue)
            .WithMany(vv => vv.ProductVariationValues)
            .HasForeignKey(pvv => pvv.VariationValueId)
            .OnDelete(DeleteBehavior.Restrict); // Specify RESTRICT to avoid cycles

        builder.Entity<VariationValue>()
            .HasMany(vv => vv.Images)
            .WithOne(pi => pi.VariationValue)
            .HasForeignKey(pi => pi.VariationValueId)
            .OnDelete(DeleteBehavior.Cascade);

        // Modify the relationship causing the cycle
        builder.Entity<VariationValue>()
            .HasOne(vv => vv.Product)
            .WithMany(p => p.VariationValues)
            .HasForeignKey(vv => vv.ProductId)
            .OnDelete(DeleteBehavior.NoAction); // Specify NO ACTION to avoid cycles
    }
}
