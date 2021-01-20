using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sludinajumi.Api.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Saites virsraksts")]
        [StringLength(60, MinimumLength = 2)]
        public string UrlTitle { get; set; }

        [Required]
        [Display(Name = "Nosaukums")]
        [StringLength(60, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Apraksts")]
        [StringLength(256, MinimumLength = 10)]
        public string Description { get; set; }

        [Display(Name = "Attēls")]
        [StringLength(80, MinimumLength = 4)]
        public string ImagePath { get; set; }

        [Display(Name = "Pamata kategorija")]
        [ForeignKey("Category")]
        public long? ParentId { get; set; }
        
        [Display(Name = "Pamata kategorija")]
        public virtual Category Parent { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual List<Ad> Ads { get; set; }
        public virtual List<CategoryProperty> Properties { get; set; }
        public virtual List<Category> Subcategories { get; set; }

        [NotMapped]
        public int AdsCount {
            get {
                if (Ads != null)
                    return Ads.Count();
                return 0;
            }
        }

        [NotMapped]
        public long PropertiesCount {
            get {
                if (Properties != null)
                    return Properties.LongCount();
                return 0;
            }
        }

        [NotMapped]
        public int SubcategoriesCount {
            get {
                if (Subcategories != null)
                    return Subcategories.Count();
                return 0;
            }
        }

        public Category()
        {
            this.CreatedAt = DateTime.Now;
        }

    }
}