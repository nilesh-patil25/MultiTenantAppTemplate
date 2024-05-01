
using MultiTenantAppTemplate.Server.Models;

namespace MultiTenancyAppTemplate.Services
{
    public class ImageService:IImageService
    {
        private readonly IConfiguration _configuration;
        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ImageFileDetail> GetImageFilesDetails(string hostName)
        {
            var basePath = _configuration.GetSection("AssetPath").Value;

            string imagesFolder = Path.Combine(basePath, hostName);
            if (!Directory.Exists(imagesFolder))
            {
                return Enumerable.Empty<ImageFileDetail>();
            }

            return Directory.GetFiles(imagesFolder).Select(file =>
            {
                var fileInfo = new FileInfo(file);
                return new ImageFileDetail
                {
                    FileName = fileInfo.Name,
                    RelativePath = fileInfo.FullName
                };
            });
        }

        public string GetContentType(string fileExtension)
        {
            // Add more content types as needed
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        public string GetBannerImagePathForTenant(string hostName, string imageType)
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

    }
}
