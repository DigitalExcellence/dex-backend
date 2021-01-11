using System;
using System.Collections.Generic;
using System.Text;
using SendGrid;

namespace NotificationSystem.Contracts
{
    public interface INotificationService
    {
        void ParsePayload(string jsonBody);
        bool ValidatePayload();
        Response ExecuteTask();
    }
}
