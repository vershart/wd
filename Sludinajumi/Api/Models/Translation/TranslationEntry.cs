using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sludinajumi.Api.Models
{
    public class TranslationEntry
    {
        [Key]
        public long Id { get; set; }
        public string NeutralValue { get; set; }
        public string NeutralDescription { get; set; }

        /// <summary>
        /// Tulkojumu izveidošanas datums
        /// </summary>
        /// <returns>izveidošanas datums</returns>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Tulkojumu pēdējas rediģēšanas datums
        /// </summary>
        /// <returns>pēdējas rediģēšanas datums</returns>
        public DateTime LastModifiedAt { get; set; }

        public List<Translation> Translations { get; set; }

        public TranslationEntry()
        {
            this.CreatedAt = DateTime.Now;
            this.LastModifiedAt = DateTime.Now;
        }

        public void Modify(TranslationEntry te)
        {
            this.NeutralValue = te.NeutralValue;
            this.NeutralDescription = te.NeutralDescription;
            this.Translations = te.Translations;
            this.LastModifiedAt = DateTime.Now;
        }

    }
}