using EmailServiceLib.Configuration.DataContracts;
using EmailServiceLib.Objects;
using EmailServiceLib.Services.DataContracts;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Linq;

namespace EmailServiceLib.Services
{
	public class MailKitEmailService : IEmailService
	{
		private readonly IEmailConfiguration _emailConfiguration;

		public MailKitEmailService(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public void Send(EmailMessage emailMessage)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(emailMessage.FromAddress.Name, emailMessage.FromAddress.Address));
			message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

			message.Subject = emailMessage.Subject;

			message.Body = new TextPart(TextFormat.Html) { Text = emailMessage.Content };

			using (var emailClient = new SmtpClient())
			{
				emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

				emailClient.Send(message);

				emailClient.Disconnect(true);
			}
		}
	}
}
