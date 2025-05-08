using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BusinessLogic.External_Service;
using Microsoft.Extensions.Options;
using BusinessLogic.Settings;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace BusinessLogic.Service
{
    public class EmailService : IEmailService
    {
        private readonly VerifyService verfiy;
        public EmailService(VerifyService verfiy)
        {
            this.verfiy = verfiy;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Here, `htmlMessage` should contain the confirmation link
            verfiy.SendEmailConfirmation(email, htmlMessage);

            // Return a completed task since SendEmailConfirmation is synchronous
            return Task.CompletedTask;
        }
    }


}
