using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models.Defaults
{
    public static class Defaults
    {
        public static class Roles
        {
            public const string BackendApplication = "BackendApplication";
            public const string Student = "Student";
            public const string StudentAndBackendApps = BackendApplication + ", " + Student;
        }

        public static class Claims
        {
            public const string StudentPcn = "studentpcn";
            public const string Classes = "classes";
            public const string IsStudentRegistered = "is_student_registered";
        }

        public static class ScopeCategories
        {
            [Description("This scope category gives read access to the Student Data namespace")]
            public const string StudentDataRead = Scopes.StudentRead;

            [Description("This scope category gives access to the Student Data namespace")]
            public const string StudentDataWrite = Scopes.StudentWrite;
        }

        public static class Scopes
        {
            [Description("This scope gives read access to the student namespace")]
            public const string StudentRead = "student:read";

            [Description("This scope gives write access to the student namespace")]
            public const string StudentWrite = "student:write";
        }
    }
}