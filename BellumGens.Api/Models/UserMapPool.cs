using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class UserMapPool
	{
		[Key]
		[Column(Order = 0)]
		public string UserId { get; set; }

		[Key]
		[Column(Order = 1)]
		public CSGOMap Map { get; set; }
		public bool IsPlayed { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}
}