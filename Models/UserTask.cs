using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Models
{
    /// <summary>
    /// user tasks with types and status
    /// </summary>
    public enum UserTaskStatus
    {
        Open,
        Completed,
        Mailed
    }

    public enum UserTaskType
    {
        GraduationReminder,
    }

    public class UserTask
    {
        public UserTask(int userId,UserTaskType type,string email)
        {
            Type = type;
            UserId = userId;
            Status = UserTaskStatus.Open;
            this.email = email;
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        public UserTaskStatus Status { get; set; }
        public UserTaskType Type { get; set; }

        public string email { get; set; }

    }

    


}
