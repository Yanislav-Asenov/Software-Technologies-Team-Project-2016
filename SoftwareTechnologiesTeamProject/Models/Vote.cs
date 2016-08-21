namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vote
    {
        [Key]
        public int Id { get; set; }

        public string VoterId { get; set; }

        [ForeignKey("VoterId")]
        public ApplicationUser Voter { get; set; }

        [Required]
        public string VoteType { get; set; }

        [Required]
        public int MatchId { get; set; }

        [ForeignKey("MatchId")]
        public Match Match { get; set; }

    }
}