using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class CSGOMatchMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public CSGOMap Map { get; set; }

        public Guid CSGOMatchId { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        [ForeignKey("CSGOMatchId")]
        public virtual TournamentCSGOMatch Match { get; set; }
    }
}