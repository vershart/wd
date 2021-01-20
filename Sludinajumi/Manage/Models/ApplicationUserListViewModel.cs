using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Sludinajumi.Manage.Models
{
    public class ApplicationUserListViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Lietotāja vārds")]
        public string FirstName { get; set; }
        [Display(Name = "Lietotāja uzvārds")]
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public string RoleName { get; set; }
        public DateTime RegistrationDate { get; internal set; }
    }
}