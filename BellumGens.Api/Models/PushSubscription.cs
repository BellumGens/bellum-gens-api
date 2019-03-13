using System;
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
}