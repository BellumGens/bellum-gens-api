using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
	public class Subscriber
	{
		[Key]
        [EmailAddress]
		public string Email { get; set; }

		public bool Subscribed { get; set; } = true;

		public Guid SubKey { get; set; } = Guid.NewGuid();
	}
}