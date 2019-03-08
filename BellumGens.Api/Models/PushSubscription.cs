using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class PushSubscription
	{
		public string endpoint { get; set; }

		public TimeSpan expirationTime { get; set; }

		[Key]
		[Column(Order = 0)]
		public string p256dh { get; set; }

		[Key]
		[Column(Order = 1)]
		public string auth { get; set; }
	}
}