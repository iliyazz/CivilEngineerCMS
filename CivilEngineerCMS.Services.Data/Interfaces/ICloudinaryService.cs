using CloudinaryDotNet.Actions;

namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file, string folder);

        Task<ImageUploadResult> UploadPhotoAsync(byte[] file, string folder);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

        bool IsFileValid(IFormFile formFile);
    }
}
