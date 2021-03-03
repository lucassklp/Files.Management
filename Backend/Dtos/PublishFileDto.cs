using System;
using Microsoft.AspNetCore.Http;

namespace Files.Management.Dtos
{
    public class PublishFileDto<IFile>
    {
        public IFile File { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
