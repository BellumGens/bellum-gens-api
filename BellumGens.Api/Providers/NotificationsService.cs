using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using WebPush;

namespace BellumGens.Api.Providers
{
	public static class NotificationsService
	{
		private static string _publicVapidKey = ConfigurationManager.AppSettings["publicVapidKey"];
		private static string _privateVapidKey = ConfigurationManager.AppSettings["privateVapidKey"];

		public static void SendNotification(List<BellumGensPushSubscription> subs, TeamInvite notification)
		{
			var subject = @"https://bellumgens.com";

			foreach (BellumGensPushSubscription sub in subs)
			{
				var subscription = new PushSubscription(sub.endpoint, sub.p256dh, sub.auth);
				var vapidDetails = new VapidDetails(subject, NotificationsService._publicVapidKey, NotificationsService._privateVapidKey);

				var webPushClient = new WebPushClient();
				var payload = new BellumGensNotificationWrapper(notification);
				try
				{
					webPushClient.SendNotification(subscription, payload.ToString(), vapidDetails);
				}
				catch (WebPushException exception)
				{
					Console.WriteLine(exception);
				}
			}
		}

		public static void SendNotification(List<BellumGensPushSubscription> subs, TeamInvite notification, NotificationState state)
		{
			var subject = @"https://bellumgens.com";

			foreach (BellumGensPushSubscription sub in subs)
			{
				var subscription = new PushSubscription(sub.endpoint, sub.p256dh, sub.auth);
				var vapidDetails = new VapidDetails(subject, NotificationsService._publicVapidKey, NotificationsService._privateVapidKey);

				var webPushClient = new WebPushClient();
				var payload = new BellumGensNotificationWrapper(notification, state);
				try
				{
					webPushClient.SendNotification(subscription, payload.ToString(), vapidDetails);
				}
				catch (WebPushException exception)
				{
					Console.WriteLine(exception);
				}
			}
		}

		public static void SendNotification(List<BellumGensPushSubscription> subs, TeamApplication notification)
		{
			var subject = @"https://bellumgens.com";

			foreach (BellumGensPushSubscription sub in subs)
			{
				var subscription = new PushSubscription(sub.endpoint, sub.p256dh, sub.auth);
				var vapidDetails = new VapidDetails(subject, NotificationsService._publicVapidKey, NotificationsService._privateVapidKey);

				var webPushClient = new WebPushClient();
				var payload = new BellumGensNotificationWrapper(notification);
				try
				{
					webPushClient.SendNotification(subscription, payload.ToString(), vapidDetails);
				}
				catch (WebPushException exception)
				{
					Console.WriteLine(exception);
				}
			}
		}

		public static void SendNotification(List<BellumGensPushSubscription> subs, TeamApplication notification, NotificationState state)
		{
			var subject = @"https://bellumgens.com";

			foreach (BellumGensPushSubscription sub in subs)
			{
				var subscription = new PushSubscription(sub.endpoint, sub.p256dh, sub.auth);
				var vapidDetails = new VapidDetails(subject, NotificationsService._publicVapidKey, NotificationsService._privateVapidKey);

				var webPushClient = new WebPushClient();
				var payload = new BellumGensNotificationWrapper(notification, state);
				try
				{
					webPushClient.SendNotification(subscription, payload.ToString(), vapidDetails);
				}
				catch (WebPushException exception)
				{
					Console.WriteLine(exception);
				}
			}
		}
	}
}