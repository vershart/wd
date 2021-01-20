using System;
using System.ComponentModel.DataAnnotations;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Models
{
    public class AdministratorRegisterViewModel : ApplicationUser
    {

        [Required]
        [Display(Name = "E-pasts")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(9)]
        public string Password { get; set; }

        [Required]
        [MinLength(9)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
 
        [Required]
        [Display(Name = "Vārds")]
        [StringLength(40)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Uzvārds")]
        [StringLength(40)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Dzimšanas datums")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}