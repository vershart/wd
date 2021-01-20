using System;
using System.Collections.Generic;
using System.Linq;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public int CategoriesCount => Categories.Count();
        public List<Ad> LatestAds { get; set; }
    }
}