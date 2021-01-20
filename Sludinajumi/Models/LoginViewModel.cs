using System;
using System.ComponentModel.DataAnnotations;

namespace Sludinajumi.Models
{
    /// <summary>
    /// Lietotāja autorizācijas modele
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Lietotāja autorizācijas vārds
        /// </summary>
        /// <returns></returns>
        [Required]
        public string UserName { get; set; }
        
        /// <summary>
        /// Lietotāja parole
        /// </summary>
        /// <returns></returns>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}