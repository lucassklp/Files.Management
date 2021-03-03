using System;
namespace Files.Management.Domain
{
    public class EditedImage
    {
        public long Id { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public long ImageId { get; set; }
        public Image Image { get; set; }
        public string Path { get; set; }
        public ImageOperation Operation { get; set; }
    }
}
