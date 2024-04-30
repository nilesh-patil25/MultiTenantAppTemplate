using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiTenantAppTemplate.Server.Controllers;
using MultiTenantAppTemplate.Server.Models;
using MultiTenantAppTemplate.Server.Services;

namespace MultiTenantAppTemplate.Test.ControllerTests
{
    public class TenantControllerTest
    {
        [Fact]
        public void should_GetAllTenant()
        {
            var mockTenantService = new Mock<ITenantService>();
            mockTenantService.Setup(service => service.GetTenants()).Returns(new List<Tenant> { new Tenant(1, "TestTenant", true, "") });

            var controller = new TenantController(mockTenantService.Object);

            // Act
            var result = controller.GetTenants();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var tenants = Assert.IsAssignableFrom<IEnumerable<Tenant>>(okResult.Value);
            Assert.NotEmpty(tenants);

        }
        [Fact]
        public void GetAllTenants_ReturnsNotFound_WhenNoTenants()
        {
            var mockTenantService = new Mock<ITenantService>();
            mockTenantService.Setup(service => service.GetTenants()).Returns(new List<Tenant> { });

            var controller = new TenantController(mockTenantService.Object);

            // Act
            var result = controller.GetTenants();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.Equal("No tenants found.", notFoundResult.Value);
        }
        [Fact]
        public void GetTenantByHost_WithValidHost_ReturnsOkResult()
        {
            string host = "TestHost";
            var mockTenantService = new Mock<ITenantService>();
            mockTenantService.Setup(x => x.GetTenantByHost(host)).Returns(new Tenant { Id = 1, Host = host });
            var controller = new TenantController(mockTenantService.Object);

            // Act
            var result = controller.GetTenantByHost(host);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tenant = Assert.IsType<Tenant>(okResult.Value);
            Assert.Equal(host, tenant.Host);
        }
    }
}