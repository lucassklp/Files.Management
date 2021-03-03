using System;
namespace Files.Management.Domain
{
    public interface IUser
    {
        long Id { get; }
        string Email { get; }
    }
}
