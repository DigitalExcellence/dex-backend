using MessagebrokerPublisher.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Contracts
{
    public interface INotificationService
    {
        bool ValidateNotification(INotification notification);
        void SendNotification(INotification notification);
    }
}
