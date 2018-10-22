﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamApplication
	{
		[Key]
		[Column(Order = 0)]
		public string ApplicantId { get; set; }

		[Key]
		[Column(Order = 1)]
		public Guid TeamId { get; set; }

		public string Message { get; set; }

		public NotificationState State { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTimeOffset Sent { get; set; }

		[ForeignKey("ApplicantId")]
		[JsonIgnore]
		public virtual ApplicationUser User { get; set; }

		[ForeignKey("TeamId")]
		[JsonIgnore]
		public virtual CSGOTeam Team { get; set; }
	}
}