using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class PushSubscription
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
}