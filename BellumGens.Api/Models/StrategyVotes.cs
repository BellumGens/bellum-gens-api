using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class StrategyVotes
	{
		[Key]
		[Column(Order = 0)]
		public Guid StratId { get; set; }

		[Key]
		[Column(Order = 1)]
		public string UserId { get; set; }

		public VoteDirection Vote { get; set; }

		[JsonIgnore]
		[ForeignKey("StratId")]
		public virtual CSGOStrategy Strategy { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}

	public enum VoteDirection
	{
		Up,
		Down
	}
}