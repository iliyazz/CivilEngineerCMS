using CloudinaryDotNet.Actions;

namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file, string folder, string publicId);

        Task<ImageUploadResult> UploadPhotoAsync(byte[] file, string folder, string publicId);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

        bool IsFileValid(IFormFile formFile);

        //string DownloadPhoto(string publicId);
    }
}
