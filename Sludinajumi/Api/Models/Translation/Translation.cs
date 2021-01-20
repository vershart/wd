using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sludinajumi.Api.Models
{
    /// <summary>
    /// Tulkojuma struktūra
    /// </summary>
    public class Translation 
    {
        /// <summary>
        /// Tulkojuma identifikators datubāzē
        /// </summary>
        /// <returns>tulkojuma identifikators</returns>
        public long Id { get; set; }
        /// <summary>
        /// Norāde uz tulkojuma valodas identifikatoru
        /// </summary>
        /// <returns>valodas identifikators</returns>
        [StringLength(2), MinLength(2), MaxLength(2)]
        public string LanguageCode { get; set; }
        /// <summary>
        /// Tulkojuma valoda
        /// </summary>
        /// <returns>valoda</returns>
        [ForeignKey("LanguageCode")]
        public virtual Language Language { get; set; }
        /// <summary>
        /// Tulkojums
        /// </summary>
        /// <returns>tulkojuma vērtība</returns>
        public string Value { get; set; }
        /// <summary>
        /// Tulkojuma izveidošanas datums
        /// </summary>
        /// <returns>izveidošanas datums</returns>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Tulkojuma pēdējas rediģēšanas datums
        /// </summary>
        /// <returns>pēdējas rediģēšanas datums</returns>
        public DateTime LastModifiedAt { get; set; }

        public long TranslationEntryId { get; set; }
        public virtual TranslationEntry TranslationEntry { get; set; }

        public Translation()
        {
            this.CreatedAt = DateTime.Now;
        }

        public void Modify(string newValue) 
        {
            this.Value = newValue;
            this.LastModifiedAt = DateTime.Now;
        }

    }
}