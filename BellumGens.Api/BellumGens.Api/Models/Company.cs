using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class Company
	{
        [Key]
        [Index(IsUnique = true)]
		public string Name { get; set; }

		public string Logo { get; set; }

        public string Website { get; set; }

        public string Description { get; set; }
	}
}