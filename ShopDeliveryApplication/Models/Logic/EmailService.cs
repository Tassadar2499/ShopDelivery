using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	public static class EmailService
	{
		private const string ADMIN_LOGIN = "meetrulet@mail.ru";
		private const string ADMIN_PASSWORD = "bratishka1234";
		private const string MAIN_ADDRESS = "smtp.mail.ru";

		public static async Task SendEmailAsync(string email, string subject, string message)
		{
			var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress("Администрация сайта по доставке продуктов", ADMIN_LOGIN));
			emailMessage.To.Add(new MailboxAddress(MAIN_ADDRESS, email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using var client = new SmtpClient();

			await client.ConnectAsync(MAIN_ADDRESS, 465, true);
			await client.AuthenticateAsync(ADMIN_LOGIN, ADMIN_PASSWORD);
			await client.SendAsync(emailMessage);
			await client.DisconnectAsync(true);
		}
	}
}