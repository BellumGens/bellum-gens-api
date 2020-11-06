using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
	public class UserMessage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public DateTimeOffset TimeStamp { get; set; }
		public string Message { get; set; }

		[ForeignKey("From")]
		public virtual ApplicationUser SendingUser { get; set; }
		[ForeignKey("To")]
		public virtual ApplicationUser ReceivingUser { get; set; }
	}
}