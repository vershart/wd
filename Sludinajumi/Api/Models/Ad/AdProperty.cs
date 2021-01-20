using System;
using System.Runtime.Serialization;

namespace Sludinajumi.Api.Models
{
    /// <summary>
    /// Sludinājuma aprakstamā objekta īpašība
    /// </summary>
    public class AdProperty
    {
        /// <summary>
        /// Sludinājuma identifikators (FK)
        /// </summary>
        /// <returns>Identifikators datu bāzē</returns>
        public long AdId { get; set; }

        /// <summary>
        /// Sludinājuma saite
        /// </summary>
        /// <returns>Sludinājuma objekta saiti</returns>
        public Ad Ad { get; set; }
        /// <summary>
        /// Īpašības identifikators (FK)
        /// </summary>
        /// <returns>Identifikatoru datu bāzē</returns>
        public long PropertyId { get; set; }

        /// <summary>
        /// Īpašības saite
        /// </summary>
        /// <returns>Īpašības objekta saiti</returns>
        public Property Property { get; set; }

        /// <summary>
        /// Īpašības vērtība
        /// </summary>
        /// <returns>Īpašības vērtību</returns>
        public string Value { get; set; }
    }
}