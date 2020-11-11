using BellumGens.Api.Core.Models;
using System;
using System.Collections.Generic;
using WebPush;

namespace BellumGens.Api.Core.Providers
{
	public static class NotificationsService
	{
		private static string _publicVapidKey = AppInfo.Config.publicVapidKey;
		private static string _privateVapidKey = AppInfo.Config.privateVapidKey;

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

		public static void SendNotification(List<BellumGensPushSubscription> subs, StrategyComment comment)
		{
			var subject = @"https://bellumgens.com";

			foreach (BellumGensPushSubscription sub in subs)
			{
				var subscription = new PushSubscription(sub.endpoint, sub.p256dh, sub.auth);
				var vapidDetails = new VapidDetails(subject, NotificationsService._publicVapidKey, NotificationsService._privateVapidKey);

				var webPushClient = new WebPushClient();
				var payload = new BellumGensNotificationWrapper(comment);
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