using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Settings;
using Microsoft.Extensions.Options;

namespace Hotel_Management.Areas.Admin.Services
{
    public class CloudinaryImageServices : IImageServices
    {
        private readonly ICloudinary _cloudinary;

        public CloudinaryImageServices(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            // Trích xuất public ID từ URL
            var uri = new Uri(imageUrl);
            var publicId = Path.GetFileNameWithoutExtension(uri.LocalPath); // Lấy phần sau cùng trước .jpg

            var deletionParams = new DeletionParams($"hotel/rooms/{publicId}");
            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName = "default")
        {
            if (file == null || file.Length == 0) return null;

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"hotel/{folderName}"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString();
        }

        public async Task<List<string>> UploadImagesAsync(List<IFormFile> files, string folderName = "default")
        {
            var urls = new List<string>();

            foreach (var file in files)
            {
                var url = await UploadImageAsync(file, folderName);
                if (url != null)
                    urls.Add(url);
            }

            return urls;
        }
    }
}
