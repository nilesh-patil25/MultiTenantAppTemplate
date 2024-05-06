using MultiTenantAppTemplate.Server.Models;

namespace MultiTenantAppTemplate.Server.Services
{
    public class TenantService : ITenantService
    {
        private readonly List<Tenant> _tenants;

        public TenantService()
        {
            _tenants = new List<Tenant>
            {
                new Tenant(1, "foo", true, "Theme1"),
                new Tenant(2, "bar", true, "Theme2")
            };
        }
        public List<Tenant> GetTenants()
        {
            return _tenants;
        }

        public Tenant GetTenantByHost(string host)
        {
            var tenant = _tenants.FirstOrDefault(t => t.Host.Equals(host, StringComparison.OrdinalIgnoreCase));
            return tenant;
        }
        public string GetBackgroundThemeForTenant(string tenantName)
        {
            if (tenantName == "foo")
                return "#a78436";
            else if (tenantName == "bar")
                return "#2d545e";
            else 
                return "#a78436";

        }
    }
            
    
}
