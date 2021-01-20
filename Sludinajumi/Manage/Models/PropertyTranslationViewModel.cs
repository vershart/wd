using System;
using System.ComponentModel.DataAnnotations;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Models
{
    public class PropertyTranslationViewModel 
    {
        public Language Language { get; set; }

        public Property Property { get; set; }
        public string TranslatedName { get; set; }
        public string TranslatedDescription { get; set; }
        
    }
}