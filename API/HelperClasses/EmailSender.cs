using MessageBrokerPublisher;
using MessageBrokerPublisher.Models;
using MessageBrokerPublisher.Services;

namespace API.HelperClasses
{
    /// <summary>
    /// EmailSender Interface
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="recipient">The email address of the recipient</param>
        /// <param name="textContent">The text content of the email</param>
        /// <param name="htmlContent">The HTML content of the email</param>
        public void Send(string recipient, string textContent, string htmlContent);
    }

    /// <summary>
    /// Class which is responsible for sending emails
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly ITaskPublisher taskPublisher;

        /// <summary>
        ///  Constructor to instantiate the email sender
        /// </summary>
        public EmailSender(ITaskPublisher taskPublisher)
        {
            this.taskPublisher = taskPublisher;
        }

        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="recipient">The email address of the recipient</param>
        /// <param name="textContent">The text content of the email</param>
        /// <param name="htmlContent">The HTML content of the email</param>
        public void Send(string recipient, string textContent, string htmlContent = null)
        {
            EmailNotificationRegister emailNotification = new EmailNotificationRegister(recipient, textContent, htmlContent);
            taskPublisher.RegisterTask(Newtonsoft.Json.JsonConvert.SerializeObject(emailNotification), Subject.EMAIL);
        }
    }
}
