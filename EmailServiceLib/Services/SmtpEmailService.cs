using EmailServiceLib.Configuration.DataContracts;
using EmailServiceLib.Objects;
using EmailServiceLib.Services.DataContracts;
using System.Net;
using System.Net.Mail;

namespace EmailServiceLib.Services
{
	public class SmtpEmailService : IEmailService
	{
		private readonly IEmailConfiguration _emailConfiguration;

		public SmtpEmailService(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public void Send(EmailMessage emailMessage)
		{
			using (var client = new SmtpClient(_emailConfiguration.SmtpServer))
			{
				client.EnableSsl = true;
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

				var mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(emailMessage.FromAddress.Address, emailMessage.FromAddress.Name);

				emailMessage.ToAddresses.ForEach(x => mailMessage.To.Add(new MailAddress(x.Address, x.Name)));

				mailMessage.Body = emailMessage.Content;
				mailMessage.Subject = emailMessage.Subject;
				mailMessage.IsBodyHtml = false;

				client.Send(mailMessage);
			}
		}
	}
}
