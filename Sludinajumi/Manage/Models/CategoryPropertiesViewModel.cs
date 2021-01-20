using System;
using System.Collections.Generic;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Models
{
    public class CategoryPropertiesViewModel
    {
        public Category Category { get; set; }
        public List<Property> AvailableProperties { get; set; }
    }
}