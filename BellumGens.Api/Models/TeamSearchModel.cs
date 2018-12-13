using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class TeamSearchModel
	{
		public string name { get; set; }
		public PlaystyleRole? role { get; set; }
		public float scheduleOverlap { get; set; }
	}
}