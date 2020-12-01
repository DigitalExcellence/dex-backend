using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public enum UserTaskStatusResourceResult
    {
        Open,
        Completed,
        Mailed
    }

    public enum UserTaskTypeResourceResult
    {
        GraduationReminder,
    }

    public class UserTaskResourceResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public UserTaskStatusResourceResult Status { get; set; }
        public UserTaskTypeResourceResult Type { get; set; }
    }
}
