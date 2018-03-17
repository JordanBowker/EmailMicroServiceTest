using EmailServiceLib.Configuration;
using EmailServiceLib.Configuration.DataContracts;
using EmailServiceLib.Objects;
using EmailServiceLib.Services;
using EmailServiceLib.Services.DataContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace EmailApp
{
	class Program
	{
		static void Main()
		{
			var configuration = GetAppConfiguration();
			var serviceProvider = RegisterServices(configuration);

			var emailMessage = configuration.GetSection("EmailMessage").Get<EmailMessage>();

			var emailService = serviceProvider.GetService<IEmailService>();
			emailService.Send(emailMessage);
		}

		protected internal static IConfigurationRoot GetAppConfiguration()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory() + "../../../../")
				.AddJsonFile("appsettings.json")
				.Build();
		}

		protected internal static ServiceProvider RegisterServices(IConfigurationRoot configuration)
		{
			var services = new ServiceCollection();
			services.AddSingleton<IEmailConfiguration>(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());

			if (bool.Parse(configuration["UseMailKit"])) services.AddTransient<IEmailService, MailKitEmailService>();
			else services.AddTransient<IEmailService, SmtpEmailService>();

			return services.BuildServiceProvider();
		}
	}
}
