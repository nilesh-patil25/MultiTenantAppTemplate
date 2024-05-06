using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAppTemplate.Server.Models;
using MultiTenantAppTemplate.Server.Services;

namespace MultiTenantAppTemplate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        #region API
        [HttpGet]
        public ActionResult<IEnumerable<Tenant>> GetTenants()
        {
            try
            {
                var tenants = _tenantService.GetTenants();
                if (tenants == null || tenants.Count == 0)
                {
                    return NotFound("No tenants found.");
                }
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tenants: {ex.Message}");
            }
        }

        [HttpGet("{host}")]
        public ActionResult<Tenant> GetTenantByHost(string host)
        {
            try
            {
                if (string.IsNullOrEmpty(host))
                {
                    return BadRequest("Host name cannot be null or empty.");
                }

                var tenant = _tenantService.GetTenantByHost(host);
                if (tenant == null)
                {
                    return NotFound($"Tenant with host '{host}' not found.");
                }

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tenant: {ex.Message}");
            }
        }


        [HttpGet("GetTenantTheme")]
        public IActionResult GetBackgroundColor(string? tenantName)
        {
            try
            {
                string backgroundColor = _tenantService.GetBackgroundThemeForTenant(tenantName);

                return Ok(new { backgroundColor });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        #endregion
    }
}
