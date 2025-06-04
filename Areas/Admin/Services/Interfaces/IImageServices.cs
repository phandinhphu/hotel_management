namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IImageServices
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName = "default");
        Task<List<string>> UploadImagesAsync(List<IFormFile> files, string folderName = "default");
        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
