using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class UserPreferencesViewModel
	{
		public string email { get; set; }

		public bool searchVisible { get; set; }
	}
}