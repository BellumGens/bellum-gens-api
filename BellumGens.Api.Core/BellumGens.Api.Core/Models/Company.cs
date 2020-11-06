using System.ComponentModel.DataAnnotations;

namespace BellumGens.Api.Core.Models
{
	public class Company
	{
        [Key]
		public string Name { get; set; }

		public string Logo { get; set; }

        public string Website { get; set; }

        public string Description { get; set; }
	}
}