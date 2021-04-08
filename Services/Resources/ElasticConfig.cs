using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Resources
{
    public class ElasticConfig
    {
        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname of the elastic instance.
        /// </value>
        [Required]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The username of the elastic instance.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password of the elastic instance.
        /// </value>
        [Required]
        public string Password { get; set; }


        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        /// <value>
        ///     The index of the elastic document.
        /// </value>
        [Required]
        public string IndexUrl { get; set; }
    }
}
