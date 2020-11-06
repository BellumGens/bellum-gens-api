using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
    public class TournamentMatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string DemoLink { get; set; }

        public string VideoLink { get; set; }

        public bool NoShow { get; set; } = false;

        public DateTimeOffset StartTime { get; set; }

        public Guid? TournamentId { get; set; }

        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }
    }
}