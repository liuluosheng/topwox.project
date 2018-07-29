using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
