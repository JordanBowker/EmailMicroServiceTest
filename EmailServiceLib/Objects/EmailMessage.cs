using System.Collections.Generic;

namespace EmailServiceLib.Objects
{
	public class EmailMessage
	{
		public List<EmailAddress> ToAddresses { get; set; }
		public EmailAddress FromAddress { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
	}
}