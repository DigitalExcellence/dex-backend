using System;
using System.Collections.Generic;
using System.Text;
using SendGrid;

namespace NotificationSystem.Contracts
{
    public interface ICallbackService
    {
        void ParsePayload(string jsonBody);
        bool ValidatePayload();
        void ExecuteTask();
    }
}
