using System;
using System.Linq;
using System.Collections.Generic;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Models
{
    public class UserViewModel
    {
        public ApplicationUser AccountInformation { get; set; }
        public string Role { get; set; }
        public List<Ad> Ads { get; set; }
        public int AdsCount
        {
            get
            {
                return Ads.Count();
            }
        }
    }
}