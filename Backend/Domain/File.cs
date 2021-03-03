using System;
using Backend.Domain;

namespace Files.Management.Domain
{
    public class File
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublic { get; set; }
        public Guid UID { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
