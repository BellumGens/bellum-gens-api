using BellumGens.Api.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BellumGens.Api.Models
{
	public class TournamentApplication
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public Guid Id { get; set; }

		public Guid TournamentId { get; set; }

        public string UserId { get; set; }

		public Guid TeamId { get; set; }

		public string CompanyId { get; set; }

		public DateTimeOffset DateSubmitted { get; set; } = DateTimeOffset.Now;

		public Game Game { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Hash { get; set; }

        public string BattleNetId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("CompanyId")]
        [JsonIgnore]
        public virtual Company Company { get; set; }

		[ForeignKey("TeamId")]
        [JsonIgnore]
        public virtual CSGOTeam Team { get; set; }

		[ForeignKey("TournamentId")]
        [JsonIgnore]
		public virtual Tournament Tournament { get; set; }

        public void UniqueCustomUrl(BellumGensDbContext context)
        {
            if (string.IsNullOrEmpty(Hash))
            {
                Hash = Util.GenerateHashString(6);
                while (context.TournamentApplications.Where(t => t.Hash == Hash).SingleOrDefault() != null)
                {
                    Hash = Util.GenerateHashString(6);
                }
            }
        }
    }
}