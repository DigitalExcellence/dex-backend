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

        public UserTask() { }

        public UserTask(User user,UserTaskType type)
        {
            Type = type;
            User = user;
            Status = UserTaskStatus.Open;
        }

        public int Id { get; set; }
        public User User { get; set; }

        public UserTaskStatus Status { get; set; }
        public UserTaskType Type { get; set; }

    }

    


}
