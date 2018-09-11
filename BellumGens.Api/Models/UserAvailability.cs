using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class UserAvailability
	{
        public UserAvailability()
        {
            From = new DateTimeOffset(new DateTime(2018, 1, 15, 0, 0, 0, DateTimeKind.Utc));
            To = new DateTimeOffset(new DateTime(2018, 1, 15, 0, 0, 0, DateTimeKind.Utc));
        }

		[JsonIgnore]
		[Key]
		[Column(Order = 0)]
		public string UserId { get; set; }

		[Key]
		[Column(Order = 1)]
		public DayOfWeek Day { get; set; }

		public bool Available { get; set; }

		public DateTimeOffset From { get; set; }

		public DateTimeOffset To { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}
}