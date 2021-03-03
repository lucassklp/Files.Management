using System;
namespace Files.Management.Dtos
{
    public class FileDto
    {
        public Guid UID { get; set; }
        public string Filename { get; set; }
        public bool IsPublic { get; set; }
        public bool IsImage { get; set; }
        public DateTime UploadAt { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
    }
}
