using System;
namespace Files.Management.Exceptions
{
    public class FileNotFoundException : BusinessException
    {
        public FileNotFoundException(Exception innerException) : base("file-not-found", "The specified file was not found", innerException)
        {
        }
    }
}
