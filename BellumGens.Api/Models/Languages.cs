using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class Languages
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string Name { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }
	}
}