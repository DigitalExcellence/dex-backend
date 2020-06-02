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

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;
using Models.Defaults;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
                                             {
                                                 new TestUser
                                                 {
                                                     SubjectId = "818727",
                                                     Username = "alice",
                                                     Password = "alice",
                                                     Claims =
                                                     {
                                                         new Claim(JwtClaimTypes.Name, "Alice Smith"),
                                                         new Claim(JwtClaimTypes.GivenName, "Alice"),
                                                         new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                                         new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                                                     }
                                                 },
                                                 new TestUser
                                                 {
                                                     SubjectId = "88421113",
                                                     Username = "bob",
                                                     Password = "bob",
                                                     Claims =
                                                     {
                                                         new Claim(JwtClaimTypes.Name, "Bob Smith"),
                                                         new Claim(JwtClaimTypes.GivenName, "Bob"),
                                                         new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                                         new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                                                     }
                                                 },
                                                 new TestUser
                                                 {
                                                     SubjectId = "985632147",
                                                     Username = "jerry",
                                                     Password = "jerry",
                                                     Claims =
                                                     {
                                                         new Claim(JwtClaimTypes.Name, "jerry Smith"),
                                                         new Claim(JwtClaimTypes.GivenName, "jerry"),
                                                         new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                                         new Claim(JwtClaimTypes.Email, "jerrySmith@email.com"),
                                                     }
                                                 },
                                                 new TestUser
                                                 {
                                                     SubjectId = "147852369",
                                                     Username = "berry",
                                                     Password = "berry",
                                                     Claims =
                                                     {
                                                         new Claim(JwtClaimTypes.Name, "berry Smith"),
                                                         new Claim(JwtClaimTypes.GivenName, "berry"),
                                                         new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                                         new Claim(JwtClaimTypes.Email, "berrySmith@email.com"),
                                                     }
                                                 }
                                             };
    }
}
