/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System.ComponentModel;

namespace Models.Defaults
{

    public static class Defaults
    {
        /// <summary>
        /// This class contains default values for a user profile when the isPublic flag of set profile is set to false
        /// </summary>
        public static class Privacy
        {
            /// <summary>
            /// The email string when user flag IsPublic is set to false.
            /// </summary>
            public const string RedactedEmail = "<Redacted>";

        }

        public class Path
        {

            public static string FilePath;

        }

        public static class Roles
        {

            public const string BackendApplication = "BackendApplication";
            public const string Student = "Student";
            public const string Teacher = "Teacher";

            public const string RegisteredUser = "RegisteredUser";
            public const string PrUser = "PrUser";
            public const string Administrator = "Administrator";
            public const string Alumni = "Alumni";
            public const string DataOfficer = "DataOfficer";

        }

        public static class Claims { }

        public static class ScopeCategories
        {

            [Description("This scope category gives read access to the entire API namespace")]
            public const string ApiDataRead = Scopes.ProjectRead + " " + Scopes.UserRead;

            [Description("This scope category gives write access to the entire API namespace")]
            public const string ApiDataWrite = Scopes.AdminProjectWrite + " " + Scopes.UserWrite;

        }

        public static class Scopes
        {

            [Description("This scope gives read access to the project namespace")]
            public const string ProjectRead = "project:read";

            [Description("This scope gives read access to the project namespace")]
            public const string ProjectWrite = "project:write";

            [Description("This scope gives read access to the user namespace")]
            public const string UserRead = "user:read";

            [Description("This scope gives write access to the user namespace")]
            public const string UserWrite = "user:write";

            [Description("This scope gives read access to the highlight namespace")]
            public const string HighlightRead = "highlight:read";

            [Description("This scope gives write access to the highlight namespace")]
            public const string HighlightWrite = "highlight:write";

            [Description("This scope gives read access to the Role namespace")]
            public const string RoleRead = "role:read";

            [Description("This scope gives write access to the Role namespace")]
            public const string RoleWrite = "role:write";

            [Description("This scope gives read access to the Category namespace")]
            public const string CategoryRead = "category:read";

            [Description("This scope gives write access to the Category namespace")]
            public const string CategoryWrite = "category:write";

            [Description("This scope gives write access to the embed namespace")]
            public const string EmbedWrite = "embed:write";

            [Description("This scope gives write access to the embed namespace")]
            public const string EmbedRead = "embed:read";

            [Description("This scope gives read access to user roles for other users within their institution to the User namespace.")]
            public const string InstitutionUserRead = "user:institution-read";

            [Description("This scope gives write access to user roles for other users within their institution to the User namespace.")]
            public const string InstitutionUserWrite = "user:institution-write";

            [Description("This scope gives write access to user roles for other users within their institution to the Project namespace.")]
            public const string InstitutionProjectWrite = "project:institution-write";

            [Description("This scope gives write access to user roles for other users within their institution to the Embed namespace.")]
            public const string InstitutionEmbedWrite = "embed:institution-write";

            [Description("This scope gives read access to the Insitution namespace.")]
            public const string InstitutionRead = "institution:read";

            [Description("This scope gives write access to the Insitution namespace.")]
            public const string InstitutionWrite = "institution:write";

            [Description("This scope gives write access to the file namespace")]
            public const string FileWrite = "file:write";

            [Description("This scope gives write access to the datasource namespace")]
            public const string DataSourceWrite = "datasource:write";

            [Description("This scope gives write access to the Call To Action option namespace")]
            public const string CallToActionOptionWrite = "callToAction:write";

            [Description("This scope gives write access to the user task namescape.")]
            public const string UserTaskWrite = "userTask:write";

            [Description("This scope gives write access to the project namespace")]
            public const string AdminProjectWrite = "adminproject:write";

        }

    }

}
