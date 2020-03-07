namespace SoftGym.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using SoftGym.Services.Data.Contracts;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly string defaultProfilePicUrl = @"https://res.cloudinary.com/dzivpr6fj/image/upload/v1580902697/ClubestPics/24029_llq8xg.png";

        public CloudinaryService(IConfiguration configuration, Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploudAsync(IFormFile file)
        {
            if (file == null || this.IsFileValid(file) == false)
            {
                return this.defaultProfilePicUrl;
            }

            string url = " ";
            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                imageBytes = stream.ToArray();
            }

            using (var uploadStream = new MemoryStream(imageBytes))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, uploadStream),
                };
                var result = await this.cloudinary.UploadAsync(uploadParams);

                url = result.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<string> UploudAsync(byte[] file)
        {
            byte[] imageBytes = file;
            string url = " ";

            using (var uploadStream = new MemoryStream(imageBytes))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.ToString(), uploadStream),
                };
                var result = await this.cloudinary.UploadAsync(uploadParams);

                url = result.Uri.AbsoluteUri;
            }

            return url;
        }

        public bool IsFileValid(IFormFile photoFile)
        {
            if (photoFile == null)
            {
                return true;
            }

            string[] validTypes = new string[]
            {
                "image/x-png", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/svg"
            };

            if (validTypes.Contains(photoFile.ContentType) == false)
            {
                return false;
            }

            return true;
        }
    }
}
