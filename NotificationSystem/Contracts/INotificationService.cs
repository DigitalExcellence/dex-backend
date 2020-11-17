using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Contracts
{
    public interface INotificationService
    {
        bool ValidateMessageBody(string body);
        bool SendNotification(string body);
    }
}
