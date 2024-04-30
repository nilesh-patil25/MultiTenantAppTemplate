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
        #endregion

    }
}
