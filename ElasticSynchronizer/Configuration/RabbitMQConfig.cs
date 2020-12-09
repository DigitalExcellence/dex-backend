using System.ComponentModel.DataAnnotations;

namespace ElasticSynchronizer.Configuration
{
    public class RabbitMQConfig
    {
        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname of the rabbit mq instance.
        /// </value>
        [Required]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The usename of the rabbit mq instance.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password of the rabbit mq instance.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}
