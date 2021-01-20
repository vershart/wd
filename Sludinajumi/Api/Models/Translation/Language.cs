using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sludinajumi.Api.Models
{
    public class Language
    {

        [Key]
        [StringLength(2), MinLength(2), MaxLength(2)]
        public string Code { get; set; }

        [Required]
        [StringLength(40), MinLength(4), MaxLength(40)]
        public string SelfName { get; set; }

        [NotMapped]
        public string NormalizedSelfName {
            get {
                if (this.SelfName != null)
                    return this.SelfName.First().ToString().ToUpper() + this.SelfName.Substring(1);
                else
                    return "";
            }
        }

        [Required]
        [StringLength(40), MinLength(4), MaxLength(40)]
        public string NeutralSelfName { get; set; }

        [NotMapped]
        public string NormalizedNeutralSelfName {
            get {
                if (this.NeutralSelfName != null)
                    return this.NeutralSelfName.First().ToString().ToUpper() + this.NeutralSelfName.Substring(1);
                else
                    return "";
            }
        }

    }
}