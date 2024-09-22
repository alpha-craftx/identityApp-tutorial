using Api.DTOs.Account;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            try
            {
                var client = new SmtpClient(_config["SmtpCredential:Host"], int.Parse(_config["SmtpCredential:Port"]))
                {
                    Credentials = new NetworkCredential(_config["SmtpCredential:Username"], _config["SmtpCredential:Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["Email:From"]),
                    Subject = emailSend.Subject,
                    Body = emailSend.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailSend.To);
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

    }
}
