using System;
using System.IO;
using System.Threading.Tasks;
using Files.Management.Dtos;

namespace Files.Management.Extensions
{
    public static class StoreFileDtoExtension
    {
        public static async Task<(string Path, Guid UID, string Extension)> CreatePhysicalFile(this StoreFileDto input)
        {
            var uid = Guid.NewGuid();
            var extension = Path.GetExtension(input.Filename);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", uid + extension);
            await File.WriteAllBytesAsync(filePath, input.File.ToArray());
            return (filePath, uid, extension);
        }
    }
}
