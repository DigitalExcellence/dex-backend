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

using System.Collections.Generic;

namespace Models
{

    public class Role
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<RoleScope> Scopes { get; set; }

    }

    public enum EnumRole
    {
        RegisteredUser = 1,
        PrUser = 2,
        DataOfficer = 3,
        Administrator = 4,
        Alumni = 5
    }

}
