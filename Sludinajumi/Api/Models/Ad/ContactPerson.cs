using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sludinajumi.Api.Models
{
    public class ContactPerson
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
    }
}