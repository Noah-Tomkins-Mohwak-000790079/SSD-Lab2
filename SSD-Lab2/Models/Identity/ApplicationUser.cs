using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SSD_Lab2.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

    }
}
