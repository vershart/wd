using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sludinajumi.Api.Models;

namespace Sludinajumi.Manage.Models
{
    public class CreateCategoryViewModel
    {

        #region Fields
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
        public long? ParentId { get; set; }
        #endregion

        #region References
            public List<Category> Parents { get; set; }

            public List<PictureInfoModel> Pictures { get; set; }
        #endregion

    }
}