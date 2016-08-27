namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bet
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int MatchId { get; set; }

        [ForeignKey("MatchId")]
        public Match Match { get; set; }
    }
}