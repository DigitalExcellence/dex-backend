using System.ComponentModel;

namespace Models.Defaults
{
    public static class Defaults
    {
        public static class Roles
        {
            public const string BackendApplication = "BackendApplication";
            public const string Student = "Student";
            public const string Teacher = "Teacher";
        }

        public static class Claims
        {
        }

        public static class ScopeCategories
        {
            [Description("This scope category gives read access to the entire API namespace")]
            public const string ApiDataRead = Scopes.ProjectRead + Scopes.UserRead;

            [Description("This scope category gives write access to the entire API namespace")]
            public const string ApiDataWrite = Scopes.ProjectWrite + Scopes.UserWrite;
        }

        public static class Scopes
        {
            [Description("This scope gives read access to the project namespace")]
            public const string ProjectRead = "project:read";

            [Description("This scope gives write access to the project namespace")]
            public const string ProjectWrite = "project:write";
            [Description("This scope gives read access to the user namespace")]
            public const string UserRead = "user:read";

            [Description("This scope gives write access to the user namespace")]
            public const string UserWrite = "user:write";
        }
    }
}