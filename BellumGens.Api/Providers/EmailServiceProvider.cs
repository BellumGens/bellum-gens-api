﻿using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
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
			msg.From = new MailAddress(ConfigurationManager.AppSettings["email"], "Bellum Gens");
			msg.Subject = message.Subject;
			msg.Body = message.Body;
			msg.IsBodyHtml = true;

			using (SmtpClient client = new SmtpClient())
			{
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailUsername"], ConfigurationManager.AppSettings["emailPassword"]);
				client.Port = 587;
				client.Host = "smtp.office365.com";
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.EnableSsl = true;
				return client.SendMailAsync(msg);
			}
		}
	}
}