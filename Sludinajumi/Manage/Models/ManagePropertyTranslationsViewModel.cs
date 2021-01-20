using System;
using System.Linq;
using System.Collections.Generic;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Models
{
    public class ManagePropertyTranslationsViewModel
    {
        public Property Property { get; set; }
        public List<Language> AvailableLanguages { get; set; }
        public int AvailableLanguagesCount { get { return this.AvailableLanguages.Count(); } }
    }
}