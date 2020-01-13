using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
    public class TournamentMatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string DemoLink { get; set; }

        public string VideoLink { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}