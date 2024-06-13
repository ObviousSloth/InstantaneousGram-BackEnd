using Microsoft.AspNetCore.Http;
using System.IO;

namespace InstantaneousGram_ImageAndVideoProcessing.Models

{
    public class FileUploadModels
    {
        public IFormFile File { get; set; }

        
    }
    public class ImageUploadModel
    {
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }

    namespace InstantaneousGram_ImageAndVideoProcessing.Models
    {
        public class VideoUploadModel
        {
            public IFormFile Video { get; set; }
            public string UserId { get; set; }
        }
    }
}
