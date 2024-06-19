using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Repositories
{
    public interface IImageRepository 
    {
        Task<string> UploadAsync(IFormFile file, int productId, MediaType mediaType, string seoFilename, string altAttribute, string titleAttribute);
        Task AddAsync(Picture picture);
        Task<Picture> GetByUrlAsync(string url);
    }
}
