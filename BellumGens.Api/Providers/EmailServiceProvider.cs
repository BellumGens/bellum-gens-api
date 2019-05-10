using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BellumGens.Api.Providers
{
	public static class EmailServiceProvider
	{
		public static Task SendConfirmationEmail(IdentityMessage message)
		{
			MailMessage msg = new MailMessage();
			msg.To.Add(new MailAddress(message.Destination));
			msg.From = new MailAddress(AppInfo.Config.email, "Bellum Gens");
			msg.Subject = message.Subject;
			msg.Body = message.Body;
			msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(AppInfo.Config.emailUsername, AppInfo.Config.emailPassword),
                Port = 587,
                Host = "smtp.office365.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            return client.SendMailAsync(msg);
		}
	}
}