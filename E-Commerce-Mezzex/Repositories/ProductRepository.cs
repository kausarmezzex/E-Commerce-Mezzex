using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_Mezzex.Data;
using E_Commerce_Mezzex.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Mezzex.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Variants).ThenInclude(v => v.Images)
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId)
        {
            return await _context.Products
                .Include(p => p.Variants).ThenInclude(v => v.Images)
                .Where(p => p.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants).ThenInclude(v => v.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants).ThenInclude(v => v.Images)
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);

                await HandleCategoriesAsync(product);

                if (product.Variants != null)
                {
                    foreach (var variant in product.Variants)
                    {
                        variant.ProductId = product.Id;  // Ensure the foreign key is set.
                        _context.ProductVariants.Add(variant);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while adding product: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Product product)
        {
            try
            {
                var existingProduct = await _context.Products
                    .Include(p => p.Categories)
                    .Include(p => p.Variants).ThenInclude(v => v.Images)
                    .FirstOrDefaultAsync(p => p.Id == product.Id);

                if (existingProduct == null)
                {
                    throw new ArgumentException($"Product with ID {product.Id} not found.");
                }

                // Update product properties
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                // Update categories
                await HandleCategoriesAsync(product);

                // Convert to List<T> here to match the method signature
                await HandleVariantsAsync(existingProduct, product.Variants.ToList());

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while updating product: {ex.Message}");
                throw;
            }
        }

        private async Task HandleCategoriesAsync(Product product)
        {
            var existingProduct = await _context.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Categories.Clear();
                foreach (var categoryId in product.CategoryId)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        existingProduct.Categories.Add(category);
                    }
                }
            }
            else
            {
                foreach (var categoryId in product.CategoryId)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                    else
                    {
                        throw new ArgumentException($"Category with ID {categoryId} not found.");
                    }
                }
            }
        }

        private async Task HandleVariantsAsync(Product existingProduct, List<ProductVariant> newVariants)
        {
            var existingVariants = existingProduct.Variants.ToList();

            foreach (var variant in newVariants)
            {
                var existingVariant = existingVariants
                    .FirstOrDefault(v => v.Id == variant.Id);

                if (existingVariant != null)
                {
                    _context.Entry(existingVariant).CurrentValues.SetValues(variant);
                    // Convert to List<T> here to match the method signature
                    await HandleVariantImagesAsync(existingVariant, variant.Images.ToList());
                }
                else
                {
                    variant.ProductId = existingProduct.Id;  // Ensure the foreign key is set.
                    _context.ProductVariants.Add(variant);
                }
            }

            // Remove old variants that are no longer associated
            var removedVariants = existingVariants
                .Where(v => !newVariants.Any(nv => nv.Id == v.Id))
                .ToList();

            foreach (var removedVariant in removedVariants)
            {
                _context.ProductVariants.Remove(removedVariant);
            }
        }

        private async Task HandleVariantImagesAsync(ProductVariant existingVariant, List<Picture> newImages)
        {
            var existingImages = existingVariant.Images.ToList();

            foreach (var image in newImages)
            {
                var existingImage = existingImages
                    .FirstOrDefault(i => i.Id == image.Id);

                if (existingImage != null)
                {
                    _context.Entry(existingImage).CurrentValues.SetValues(image);
                }
                else
                {
                    image.ProductVariantId = existingVariant.Id;
                    _context.Pictures.Add(image);
                }
            }

            // Remove old images that are no longer associated
            var removedImages = existingImages
                .Where(i => !newImages.Any(ni => ni.Id == i.Id))
                .ToList();

            foreach (var removedImage in removedImages)
            {
                _context.Pictures.Remove(removedImage);
            }
        }
    }
}
