using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sludinajumi.Api.Models
{
    public class Ad
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Kategorija")]
        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = "Sludinājuma teksta valoda")]
        [ForeignKey("Language")]
        [StringLength(2), MinLength(2), MaxLength(2)]
        public string LanguageCode { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string CreatedById { get; set; }

        [Required]
        [Display(Name = "Virsraksts")]
        [StringLength(60, MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Sludinājuma teksts")]
        [StringLength(1000, MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Atrašanas vieta")]
        [StringLength(100, MinimumLength = 4)]
        public string ItemLocation { get; set; }

        [Required]
        public DateTime CreatedAt { get; }

        public ApplicationUser CreatedBy { get; set; }
        public virtual Language Language { get; set; }
        public List<AdProperty> ItemProperties { get; set; }
        public virtual Category Category { get; set; }

        public Ad()
        {
            this.CreatedAt = DateTime.Now;
        }

    }
}