﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class UserMapPool
	{
		[JsonIgnore]
		[Key]
		[Column(Order = 0)]
		public string UserId { get; set; }

		[Key]
		[Column(Order = 1)]
		public CSGOMaps Map { get; set; }
		public bool IsPlayed { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}

	public enum CSGOMaps
	{
		Cache,
		Dust2,
		Inferno,
		Mirage,
		Nuke,
		Overpass,
		Train
	}

	//public class CSGOMap
	//{
	//	public CSGOMaps Id { get; set; }
	//	public string Name { get; set; }
	//}
}