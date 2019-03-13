using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class BellumGensPushSubscription
	{
		[Key]
		public string userId { get; set; }
		public string endpoint { get; set; }

		public TimeSpan expirationTime { get; set; }
		
		public string p256dh { get; set; }

		public string auth { get; set; }

		[ForeignKey("userId")]
		public virtual ApplicationUser User { get; set; }
	}

	public class BellumGensPushSubscriptionViewModel
	{
		public string endpoint { get; set; }

		public TimeSpan expirationTime { get; set; }

		public Keys keys { get; set; }

		public class Keys
		{
			public string p256dh { get; set; }

			public string auth { get; set; }
		}
	}

	public class BellumGensNotificationWrapper
	{
		public BellumGensNotificationWrapper(TeamInvite invite)
		{
			notification = new BellumGensNotification()
			{
				title = $"You have been invited to join team {invite.TeamInfo.TeamName}",
				image = invite.TeamInfo.TeamAvatar,
				data = invite,
				renotify = true,
				actions = new List<BellumGensNotificationAction>()
				{
					new BellumGensNotificationAction()
					{
						action = "viewteam",
						title = "View team"
					}
				}
			};
		}

		public BellumGensNotification notification { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

	public class BellumGensNotification
	{
		public string title { get; set; }

		public string dir { get; set; } = "ltr";

		public string lang { get; set; } = "en";

		public string badge { get; set; } = "https://bellumgens.com/assets/icons/icon-72x72.png";

		public string icon { get; set; } = "https://bellumgens.com/assets/icons/icon-144x144.png";

		public string tag { get; set; }

		public string image { get; set; }

		public object data { get; set; }

		public int[] vibrate { get; set; } = { 200, 100, 200 };

		public bool renotify { get; set; }

		public bool requireInteraction { get; set; }

		public List<BellumGensNotificationAction> actions { get; set; }
	}

	public class BellumGensNotificationAction
	{
		public string action { get; set; }

		public string title { get; set; }

		public string icon { get; set; }
	}
}