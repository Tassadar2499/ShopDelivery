using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
    public class EmailService
    {
        private const string ADMIN_LOGIN = "meetrulet@mail.ru";
        private const string ADMIN_PASSWORD = "bratishka1234";
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            var mainAddress = "smtp.mail.ru";

            emailMessage.From.Add(new MailboxAddress("Администрация сайта по доставке продуктов", ADMIN_LOGIN));
            emailMessage.To.Add(new MailboxAddress(mainAddress, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

			using var client = new SmtpClient();

			await client.ConnectAsync(mainAddress, 465, true);
			await client.AuthenticateAsync(ADMIN_LOGIN, ADMIN_PASSWORD);
			await client.SendAsync(emailMessage);
			await client.DisconnectAsync(true);
		}
    }
}
