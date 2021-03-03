using System;
using System.IO;

namespace Files.Management.Dtos
{
    public class StoreFileDto : PublishFileDto<MemoryStream>
    {
        public string Filename { get; set; }
    }
}
