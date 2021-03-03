using System;
using System.Collections.Generic;

namespace Files.Management.Domain
{
    public class Image : File
    {
        public long Width { get; set; }
        public long Height { get; set; }

        public List<EditedImage> EditedImages { get; set; }
    }
}
