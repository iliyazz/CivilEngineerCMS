using CivilEngineerCMS.Common;
using CivilEngineerCMS.Services.Data.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace CivilEngineerCMS.Services.Data
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile formFile, string folder)
        {
            ImageUploadResult uploadResult = new ImageUploadResult();
            if (formFile.Length > 0)
            {
                await using Stream stream = formFile.OpenReadStream();
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(formFile.FileName, stream),
                    Folder = folder,
                    //Type = Image,
                    //Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };
                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(byte[] file, string folder)
        {
            byte[] imageBytes = file;
            ImageUploadResult uploadResult = new ImageUploadResult();
            if (imageBytes.Length > 0)
            {
                await using MemoryStream stream = new MemoryStream(imageBytes);
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.ToString(), stream),
                    Folder = folder,
                    //Type = "upload",
                    //Type = "private",
                    



                    //Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };
                uploadResult = await this.cloudinary.UploadAsync(uploadParams);

            }
            return uploadResult;
        }
        
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            DeletionParams deletionParams = new DeletionParams(publicId)
            {
                PublicId = publicId,
                Type = "upload",
                ResourceType = ResourceType.Image
            };
            DeletionResult result = await this.cloudinary.DestroyAsync(deletionParams);
            return result;
        }

        public bool IsFileValid(IFormFile formFile)
        {

            if (formFile == null)
            {
                return false;
            }

            if (formFile.Length > GeneralApplicationConstants.ImageMaxSizeInBytes)
            {
                //this.ModelState.AddModelError(nameof(this.Input.Image), "File size must be less than 10 MB");
                return false;
            }

            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
            string ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                //this.ModelState.AddModelError(nameof(this.Input.Image), "Invalid file extension.");
                return false;
            }

            return true;
        }
    }
}
