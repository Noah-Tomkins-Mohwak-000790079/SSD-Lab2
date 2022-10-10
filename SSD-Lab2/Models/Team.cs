using System.ComponentModel.DataAnnotations;

namespace SSD_Lab2.Models
{
    public class Team
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string? TeamName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Display(Name = "Established Date")]
        public DateTime? EstablishedDate { get; set; }

    }
}
