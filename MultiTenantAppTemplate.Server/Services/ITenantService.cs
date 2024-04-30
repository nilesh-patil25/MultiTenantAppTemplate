using MultiTenantAppTemplate.Server.Models;

namespace MultiTenantAppTemplate.Server.Services
{
    public interface ITenantService
    {
        List<Tenant> GetTenants();
        Tenant GetTenantByHost(string host);
    }
}