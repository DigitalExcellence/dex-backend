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

        public static class Roles
        {

            public const string BackendApplication = "BackendApplication";
            public const string Student = "Student";
            public const string Teacher = "Teacher";

        }

        public static class Claims { }

        public static class ScopeCategories
        {

            [Description("This scope category gives read access to the entire API namespace")]
            public const string ApiDataRead = Scopes.ProjectRead + " " + Scopes.UserRead;

            [Description("This scope category gives write access to the entire API namespace")]
            public const string ApiDataWrite = Scopes.ProjectWrite + " " + Scopes.UserWrite;

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

            [Description("This scope gives read access to the highlight namespace")]
            public const string HighlightRead = "highlight:read";

            [Description("This scope gives write access to the highlight namespace")]
            public const string HighlightWrite = "highlight:write";

        }

    }

}
