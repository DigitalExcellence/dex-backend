using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// user tasks with types and status
    /// </summary>
    public enum UserTaskStatus
    {
        open,
        completed
    }

    public enum UserTaskType
    {
        graduationReminder,
    }

    public class UserTask
    {
        public UserTask(int userId,UserTaskType type)
        {
            Type = type;
            UserId = userId;
            Status = UserTaskStatus.open;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public UserTaskStatus Status { get; set; }
        public UserTaskType Type { get; set; }
    }


}
