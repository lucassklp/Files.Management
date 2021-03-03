using System;
namespace Files.Management.Exceptions
{
    public class MailNotFoundException : BusinessException
    {
        public MailNotFoundException() : base("mail-not-found", "Mail configuration not found for this user")
        {
        }
    }
}
