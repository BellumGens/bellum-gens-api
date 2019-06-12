﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class StrategyComment
	{
		public Guid Id { get; set; }

		public Guid StratId { get; set; }

		public string UserId { get; set; }

		public DateTimeOffset Published { get; set; } = DateTimeOffset.Now;

		public string Comment { get; set; }

		[JsonIgnore]
		[ForeignKey("StratId")]
		public virtual TeamStrategy Strategy { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}
}