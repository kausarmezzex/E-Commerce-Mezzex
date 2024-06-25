using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using E_Commerce_Mezzex.Models.Domain;
using Microsoft.EntityFrameworkCore;
using E_Commerce_Mezzex.Data;

namespace E_Commerce_Mezzex.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;
        private readonly Cloudinary client;
        private readonly ApplicationDbContext dbContext;

        public CloudinaryImageRepository(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
            );
            client = new Cloudinary(account);
        }

        public async Task<string> UploadAsync(IFormFile file, int productId, MediaType mediaType, string seoFilename, string altAttribute, string titleAttribute)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            if (mediaType != MediaType.Image)
            {
                uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    DisplayName = file.FileName
                };
            }
            else if (mediaType == MediaType.Video)
            {
                uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    DisplayName = file.FileName
                };
            }

            try
            {
                var uploadResult = await client.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    return uploadResult.SecureUri.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task AddAsync(Picture picture)
        {
            try
            {
                dbContext.Pictures.Add(picture);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine("Error occurred while saving the entity changes:");
                Console.WriteLine(ex.Message);

                // If InnerException is present, log its details as well
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.Message);
                }

                // Handle or rethrow the exception as needed
                throw; // Rethrow the exception to propagate it up the call stack
            }
        }

        public async Task<Picture> GetByUrlAsync(string url)
        {
            return await dbContext.Pictures.FirstOrDefaultAsync(p => p.VirtualPath == url);
        }
    }
}
