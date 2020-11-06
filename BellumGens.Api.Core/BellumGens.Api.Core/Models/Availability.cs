using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
	public class Availability
	{
		public Availability()
		{
			From = new DateTimeOffset(new DateTime(2018, 1, 15, 0, 0, 0, DateTimeKind.Utc));
			To = new DateTimeOffset(new DateTime(2018, 1, 15, 0, 0, 0, DateTimeKind.Utc));
		}

		public bool Available { get; set; }

		public DateTimeOffset From { get; set; }

		public DateTimeOffset To { get; set; }
	}
}