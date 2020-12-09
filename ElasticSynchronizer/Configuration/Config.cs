using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace ElasticSynchronizer.Configuration
{
    public class Config : IValidatable
    {
        /// <summary>
        ///     Gets or sets the Elastic config.
        /// </summary>
        /// <value>
        ///     The Elastic config.
        /// </value>
        public ElasticConfig Elastic { get; set; }

        /// <summary>
        ///     Gets or sets the rabbit mq config.
        /// </summary>
        /// <value>
        ///     The rabbit mq config.
        /// </value>
        public RabbitMQConfig RabbitMQ { get; set; }

        /// <summary>
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(RabbitMQ, new ValidationContext(RabbitMQ), true);
            Validator.ValidateObject(Elastic, new ValidationContext(Elastic), true);
        }
    }
}
