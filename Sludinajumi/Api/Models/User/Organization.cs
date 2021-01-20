using System;
using System.ComponentModel.DataAnnotations;

namespace Sludinajumi.Api.Models
{

    public enum OrganizationType
    {
        AS         = 1,
        PS         = 2,
        SIA        = 3,
        Individual = 4
    }

    public class Organization
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public OrganizationType Type { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string LegalAdress { get; set; }
        [Required]
        public DateTime RegistationDate { get; set; }
        public string Website { get; set; }

        public bool IsConfirmed { get; set; }

        public long OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

    }
}