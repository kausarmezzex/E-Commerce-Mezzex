using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Models.ViewModel;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace E_Commerce_Mezzex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariationsImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public VariationsImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost("SaveAllAsync")]
        public async Task<IActionResult> SaveVariationAllAsync([FromBody] List<ImageViewModel> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("No image details received");
            }

            try
            {
                foreach (var image in images)
                {
                    var picture = new Picture
                    {
                        VirtualPath = image.VirtualPath,
                        ProductId = image.ProductId,
                        VariationValueId = image.VariationValueId, // Assign VariationValueId from ViewModel
                        SeoFilename = image.SeoFilename,
                        AltAttribute = image.AltAttribute,
                        TitleAttribute = image.TitleAttribute,
                        MediaType = image.MediaType
                    };

                    await imageRepository.AddAsync(picture);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (ensure you have a logging mechanism)
                Console.Error.WriteLine(ex);
                return StatusCode(500, "An error occurred while saving the images.");
            }
        }
    }
}
