using MultiTenantAppTemplate.Server.Models;

namespace MultiTenancyAppTemplate.Services
{
    public interface IImageService
    {
        IEnumerable<ImageFileDetail> GetImageFilesDetails(string hostName);
        string GetContentType(string fileExtension);
        string GetBannerImagePathForTenant(string hostName, string imageType);

    }
}