using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Sludinajumi.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
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

        [Display(Name = "Organizācija")]
        public long? OrganizationId { get; set; }

        [Required]
        [Display(Name = "Reģistrācijas datums")]
        public DateTime RegistrationDate { get; set; }

        [NotMapped]
        public string FullName {
            get {
                return $"{FirstName} {LastName}";
            }
        }

        [Display(Name = "Organizācija")]
        public virtual Organization Organization { get; set; }
        [Display(Name = "Lietotāja sludinājumi")]
        public virtual List<Ad> UserAds { get; set; }

        public ApplicationUser()
        {
            this.RegistrationDate = DateTime.Now;
        }

    }
}