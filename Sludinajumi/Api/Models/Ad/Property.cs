using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sludinajumi.Api.Models
{
    /// <summary>
    /// Objekta īpašība
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Īpašības identifikators datubāzē
        /// </summary>
        /// <returns>īpašības identifikators</returns>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Norāde uz īpašības nosaukumu (tulkojuma struktūru)
        /// </summary>
        /// <returns>norādi uz īpašības nosaukumu</returns>
        [Required]
        public long NameId { get; set; }

        /// <summary>
        /// Norāde uz īpašības aprakstu (tulkojuma struktūru)
        /// </summary>
        /// <returns>norādi uz īpašības aprakstu</returns>
        [Required]
        public long DescriptionId { get; set; }

        /// <summary>
        /// Norāda uz to, ka īpašība ir obligāta aizpildīšanai sludinājuma izveidošanas procesā
        /// </summary>
        /// <returns>īpašība ir vai nav obligāta aizpildīšanai</returns>
        [Required]
        public bool IsRequired { get; set; }


        /// <summary>
        /// Īpašības nosaukums
        /// </summary>
        /// <returns>īpašības nosaukumu</returns>
        public virtual TranslationEntry Name { get; set; }

        /// <summary>
        /// Īpašības apraksts
        /// </summary>
        /// <returns>īpašības aprakstu</returns>
        public virtual TranslationEntry Description { get; set; }

        /// <summary>
        /// Kategorijas, kas ir saistītas ar īpašību
        /// </summary>
        /// <returns>Kategorijas, kas ir saistītas ar īpašību</returns>
        public virtual List<CategoryProperty> Categories { get; set; }

        public long CategoriesCount {
            get {
                if (Categories != null)
                    return Categories.LongCount();
                return 0;
            }
        }

    }
}