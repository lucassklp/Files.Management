using System;
using Files.Management.Domain;

namespace Files.Management.Dtos
{
    public class JwtUserDto : IUser
    {
        public JwtUserDto(long id, string email)
        {
            Id = id;
            Email = email;
        }

        public string Email { get; private set; }
        public long Id { get; private set; }
    }
}
