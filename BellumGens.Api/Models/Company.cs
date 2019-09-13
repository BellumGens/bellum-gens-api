using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class Company
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Logo { get; set; }
	}
}