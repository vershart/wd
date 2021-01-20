using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Models
{
    public class NewCategoryPropertyViewModel
    {
        public string UrlTitle { get; set; }
        public long CategoryId { get; set; }
        
        [Display(Name = "Īpašības nosaukums (neitrāla valodā)")]
        public string PropertyNeutralValue { get; set; }
        [Display(Name = "Īpašības apraksts (neitrālā valodā)")]
        public string PropertyNeutralDescription { get; set; }
        [Display(Name = "Īpašība ir obligāta aizpildīšanai šīnī kategorijā")]
        public bool PropertyIsRequired { get; set; }

    }
}