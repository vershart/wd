using System;
using Microsoft.AspNetCore.Http;

namespace Sludinajumi.Manage.Models
{
    public class ImageUploadModel 
    {
        public IFormFile UploadedFile { get; set; }
        public string FileName { get; set; }
    }
}