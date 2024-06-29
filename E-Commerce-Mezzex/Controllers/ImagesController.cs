using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file, int productId, MediaType mediaType)
        {
            try
            {
                var mediaUrl = await _imageRepository.UploadAsync(file, productId, mediaType, null, null, null);
                if (mediaUrl == null)
                {
                    return Problem("Something went wrong during media upload.", null, (int)HttpStatusCode.InternalServerError);
                }

                return new JsonResult(new { link = mediaUrl });
            }
            catch (Exception ex)
            {
                // Log the exception (ensure you have a logging mechanism)
                Console.Error.WriteLine(ex);
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "An error occurred during media upload.",
                    status = 500
                });
            }
        }
    }
}