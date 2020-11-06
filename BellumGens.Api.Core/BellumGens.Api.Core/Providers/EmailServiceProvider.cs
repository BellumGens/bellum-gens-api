using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BellumGens.Api.Core.Providers
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

        public static Task SendNotificationEmail(string destination, string subject, string body, string bcc = "info@eb-league.com")
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(destination));
            msg.Bcc.Add(new MailAddress(bcc));
            msg.From = new MailAddress(AppInfo.Config.email, "Bellum Gens");
            msg.Subject = subject;
            msg.Body = body;
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