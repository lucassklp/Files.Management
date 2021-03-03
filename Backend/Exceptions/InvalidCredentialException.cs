using System;
namespace Files.Management.Exceptions
{
    public class InvalidCredentialException : BusinessException
    {
        public InvalidCredentialException() :
            base("invalid-credential", "You provided a invalid credential")
        {
        }
    }
}
