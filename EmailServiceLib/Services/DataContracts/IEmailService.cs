using EmailServiceLib.Objects;

namespace EmailServiceLib.Services.DataContracts
{
	public interface IEmailService
	{
		void Send(EmailMessage emailMessage);
	}
}
