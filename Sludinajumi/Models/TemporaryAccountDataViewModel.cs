using System;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Models
{
    public class TemporaryAccountDataViewModel
    {
        public ApplicationUser ApplicationUserData { get; }
        public string Password { get; }

        public TemporaryAccountDataViewModel(ApplicationUser au, string password)
        {
            this.ApplicationUserData = au;
            this.Password = password;
        }

    }
}