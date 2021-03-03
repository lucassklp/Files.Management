using System;
using System.Collections.Generic;
using Files.Management.Domain;

namespace Backend.Domain
{
    public class User : IUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string Password { get; set; }

        public List<File> Files { get; set; }
    }
}