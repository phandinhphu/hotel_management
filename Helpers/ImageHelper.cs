using static Hotel_Management.Helpers.ImageHelper;

namespace Hotel_Management.Helpers
{
    public class ImageHelper
    {
        private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2 MB
        private readonly string _defaultFolder = "wwwroot/images";

        /// <summary>
        /// Kiểm tra và lưu một hình ảnh vào thư mục (/wwwroot/img/{folderName})
        /// </summary>
        /// <param name="image">File ảnh cần lưu</param>
        /// <param name="folderName">Thư mục con để lưu ảnh</param>
        /// <returns>Tên file đã lưu</returns>
        public string SaveImage(IFormFile image, string folderName = "default")
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentNullException(nameof(image), "File ảnh không được để trống.");
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), _defaultFolder, folderName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var extension = Path.GetExtension(image.FileName).ToLower();

            // Kiểm tra đuôi file
            if (!_allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Chỉ lưu file có đuôi .jpg, .jpeg, .png, .gif.");
            }

            // Kiểm tra kích thước file
            if (image.Length > _maxFileSize)
            {
                throw new InvalidOperationException("File phải nhỏ hơn 2 MB.");
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadFolder, fileName);

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName;
        }

        /// <summary>
        /// Xóa một hình ảnh từ thư mục (/wwwroot/img/{folderName})
        /// </summary>
        /// <param name="fileName">Tên file cần xóa</param>
        /// <param name="folderName">Thư mục con chứa ảnh</param>
        /// <returns>true nếu xóa thành công, false nếu không</returns>
        public bool DeleteImage(string fileName, string folderName = "default")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "Tên file không được để trống.");
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), _defaultFolder, folderName);
            var extension = Path.GetExtension(fileName).ToLower();

            // Kiểm tra đuôi file
            if (!_allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Đuôi file không hợp lệ.");
            }

            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể xóa file {fileName}: {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra và lưu hình ảnh vào thư mục (/wwwroot/img/{folderName})
        /// </summary>
        /// <param name="images"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public List<string> SaveImages(List<IFormFile> images, string folderName = "default")
        {
            var imagePaths = new List<string>();
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), _defaultFolder, folderName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                // Kiểm tra đuôi file
                if (!_allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Chỉ lưu file có đuôi .jpg, .jpeg, .png, .gif.");
                }

                // Kiểm tra kích thước file
                if (image.Length > _maxFileSize)
                {
                    throw new InvalidOperationException("Mỗi file phải nhỏ hơn 2 MB.");
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadFolder, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                imagePaths.Add(fileName);
            }

            return imagePaths;
        }

        /// <summary>
        /// Xóa hình ảnh từ thư mục (/wwwroot/img/{folderName})
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public int DeleteImages(List<string> images, string folderName = "default")
        {
            if (images == null || !images.Any())
            {
                throw new ArgumentNullException(nameof(images), "Danh sách tên file không được để trống.");
            }
            // Loại bỏ các tên file trùng lặp
            images = images.Distinct().ToList();
            int deletedCount = 0;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), _defaultFolder, folderName);

            foreach (var fileName in images)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    continue; // Bỏ qua nếu tên file rỗng
                }

                var extension = Path.GetExtension(fileName).ToLower();
                if (!_allowedExtensions.Contains(extension))
                {
                    continue; // Bỏ qua nếu đuôi file không hợp lệ
                }

                var filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                        deletedCount++; // Tăng số đếm khi xóa thành công
                    }
                    catch (Exception ex)
                    {
                        // Bỏ qua lỗi riêng lẻ và tiếp tục với file tiếp theo
                        Console.WriteLine($"Không thể xóa file {fileName}: {ex.Message}");
                    }
                }
            }

            return deletedCount; // Trả về tổng số file đã xóa thành công
        }


    }
}
