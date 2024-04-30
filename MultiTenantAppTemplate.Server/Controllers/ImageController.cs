using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAppTemplate.Server.Services;

namespace MultiTenantAppTemplate.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IConfiguration _configuration;

        public ImageController(ITenantService tenantService, IConfiguration configuration)
        {
            _tenantService = tenantService;
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

                string imagePath = GetBannerImagePathForTenant(hostName, "banner");

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

                string imagePath = GetBannerImagePathForTenant(hostName, "favicon");

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

        #endregion


        #region Methods
        private string GetBannerImagePathForTenant(string hostName, string imageType)
        {
            try
            {
                var basePath = _configuration.GetSection("AssetPath").Value;
                var fileName = "";

                // This is just a placeholder, replace it with your actual logic
                string imagesFolder = Path.Combine(basePath, hostName);
                if (!Directory.Exists(imagesFolder))
                {
                    return string.Empty;
                }
                string[] imageFiles = Directory.GetFiles(imagesFolder);
                if (imageFiles.Length == 0)
                {
                    return string.Empty;
                }
                foreach (var imageFile in imageFiles)
                {
                    if (!string.IsNullOrEmpty(imageFile))
                    {
                        if (imageFile.Contains(imageType))
                        {
                            return Path.Combine(imagesFolder, imageFile);
                        }
                    }
                }
                throw new FileNotFoundException("Banner image not found for tenant.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the image path for tenant");
            }
        }
        #endregion

    }
}
