using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenancyAppTemplate.Services;
using MultiTenantAppTemplate.Server.Services;
using System.Net;

namespace MultiTenantAppTemplate.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public ImageController(ITenantService tenantService, IImageService imageService,
        IConfiguration configuration)
        {
            _tenantService = tenantService;
            _imageService = imageService;
            _configuration = configuration;
        }
        #region API
        [HttpPost("{host}/banner")]
        public async Task<ActionResult> UploadBannerImage(string host, IFormFile file)
        {
            try
            {
                var tenant = _tenantService.GetTenantByHost(host);
                if (tenant == null)
                {
                    return NotFound($"Tenant with host '{host}' not found.");
                }
                var basePath = _configuration.GetSection("AssetPath").Value;
                var uploadsPath = Path.Combine(basePath, host);
                Directory.CreateDirectory(uploadsPath);

                var fileName = $"banner_{file.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("Home banner uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading home banner: {ex.Message}");
            }
        }

        [HttpPost("{host}/favicon")]
        public async Task<ActionResult> UploadFaviconImage(string host, IFormFile file)
        {
            try
            {
                var tenant = _tenantService.GetTenantByHost(host);
                if (tenant == null)
                {
                    return NotFound($"Tenant with host '{host}' not found.");
                }

                var basePath = _configuration.GetSection("AssetPath").Value;
                var uploadsPath = Path.Combine(basePath, host);
                Directory.CreateDirectory(uploadsPath);

                var fileName = $"favicon_{file.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("Favicon uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading favicon: {ex.Message}");
            }
        }

        [HttpGet("downloadBannerImage")]
        public IActionResult DownloadBannerImage(string hostName)
        {
            try
            {
                if (string.IsNullOrEmpty(hostName))
                {
                    return BadRequest("Tenant name cannot be null or empty.");
                }

                string imagePath = _imageService.GetBannerImagePathForTenant(hostName, "banner");

                if (string.IsNullOrEmpty(imagePath) || !System.IO.File.Exists(imagePath))
                {
                    return NotFound($"Image file not found for tenant '{hostName}'.");
                }

                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);


                string contentType = "image/jpeg"; // Assume JPEG format


                return File(imageBytes, contentType, $"{hostName}_banner_image.jpg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while downloading image: {ex.Message}");
            }
        }

        [HttpGet("downloadFaviconImage")]
        public IActionResult DownloadFaviconImage(string hostName)
        {
            try
            {
                if (string.IsNullOrEmpty(hostName))
                {
                    return BadRequest("Tenant name cannot be null or empty.");
                }

                string imagePath = _imageService.GetBannerImagePathForTenant(hostName, "favicon");

                if (string.IsNullOrEmpty(imagePath) || !System.IO.File.Exists(imagePath))
                {
                    return NotFound($"Image file not found for tenant '{hostName}'.");
                }

                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);


                string contentType = "image/jpeg"; // Assume JPEG format


                return File(imageBytes, contentType, $"{hostName}_banner_image.jpg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while downloading image: {ex.Message}");
            }
        }


        [HttpGet("LoadImagePaths")]
        public IActionResult LoadPath(string hostName)
        {
            try
            {
                var imageFiles = _imageService.GetImageFilesDetails(hostName);
                if (imageFiles == null || !imageFiles.Any())
                {
                    return NotFound($"No images found for tenant '{hostName}'.");
                }

                return Ok(imageFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving images: {ex.Message}");
            }
        }

        [HttpGet("GetFileFromPath")]
        public IActionResult GetFile(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    return BadRequest("File path cannot be null or empty.");
                }

                if (!System.IO.File.Exists(relativePath))
                {
                    return NotFound($"File not found at path '{relativePath}'.");
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(relativePath);

                string contentType = "application/octet-stream"; // Default content type
                if (Path.HasExtension(relativePath))
                {
                    string fileExtension = Path.GetExtension(relativePath);
                    contentType = _imageService.GetContentType(fileExtension);
                }

                // Return the file as a download
                return File(fileBytes, contentType, Path.GetFileName(relativePath));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while downloading file: {ex.Message}");
            }
        }
        #endregion
    }
}
