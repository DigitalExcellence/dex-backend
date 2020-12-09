using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Contracts
{
    public interface INotificationService
    {
        void ParsePayload(string jsonBody);
        bool ValidatePayload();
        void ExecuteTask();
    }
}
