using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// Call To Actions with types and status
    /// </summary>
    public enum CallToActionStatus
    {
        open,
        completed
    }

    public enum CallToActionType
    {
        graduationReminder,
    }

    public class CallToAction
    {
        public CallToAction(int userId,CallToActionType type)
        {
            Type = type;
            UserId = userId;
            Status = CallToActionStatus.open;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public CallToActionStatus Status { get; set; }
        public CallToActionType Type { get; set; }
    }


}
