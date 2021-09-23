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

using Models;
using Serilog;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer
{

    public static class TestUsers
    {

        /// <summary>
        ///     Gets the default users.
        /// </summary>
        /// <returns>The list of default identity users.</returns>
        public static List<IdentityUser> GetDefaultIdentityUsers()
        {
            List<IdentityUser> users = new List<IdentityUser>
                                       {
                                           new IdentityUser
                                           {
                                               SubjectId = "818727",
                                               Username = "alice",
                                               Password = LoginHelper.GetHashPassword("alice"),
                                               Name = "Alice Smith",
                                               Firstname = "Alice",
                                               Lastname = "Smith",
                                               Email = "AliceSmith@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "88421113",
                                               Username = "bob",
                                               Password = LoginHelper.GetHashPassword("bob"),
                                               Name = "Bob Smith",
                                               Firstname = "Bob",
                                               Lastname = "Smith",
                                               Email = "BobSmith@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "985632147",
                                               Username = "jerry",
                                               Password = LoginHelper.GetHashPassword("jerry"),
                                               Name = "jerry Smith",
                                               Firstname = "jerry",
                                               Lastname = "Smith",
                                               Email = "jerrySmith@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "147852369",
                                               Username = "berry",
                                               Password = LoginHelper.GetHashPassword("berry"),
                                               Name = "berry Smith",
                                               Firstname = "berry",
                                               Lastname = "Smith",
                                               Email = "berrySmith@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "14785236923",
                                               Username = "dex",
                                               Password = LoginHelper.GetHashPassword("dex"),
                                               Name = "DeX User",
                                               Firstname = "DeX",
                                               Lastname = "User",
                                               Email = "dex@dex.software"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "954654861",
                                               Username = "john",
                                               Password = LoginHelper.GetHashPassword("john"),
                                               Name = "John Smith",
                                               Firstname = "John",
                                               Lastname = "Smith",
                                               Email = "johnSmith@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "123456789",
                                               Username = "AlumniTest",
                                               Password = LoginHelper.GetHashPassword("AlumniTest"),
                                               Name = "Alumni Test",
                                               Firstname = "Alumni",
                                               Lastname = "Test",
                                               Email = "alumnirole@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "32423446",
                                               Username = "ElasticAdmin",
                                               Password = LoginHelper.GetHashPassword("Admin321!"),
                                               Name = "Elastic",
                                               Firstname = "Admin",
                                               Lastname = "Admin",
                                               Email = "Admin@email.com"
                                           },
                                           new IdentityUser
                                           {
                                               SubjectId = "74489498",
                                               Username = "DeXAdmin",
                                               Password = LoginHelper.GetHashPassword("Admin321!"),
                                               Name = "Admin",
                                               Firstname = "DeX",
                                               Lastname = "Admin",
                                               Email = "DeXAdmin@email.com"
                                           }
                                       };

            return users;
        }

        /// <summary>
        ///     Creates a password for a test user and logs it into the console.
        /// </summary>
        /// <returns>The hashed password/.</returns>
        public static string CreateTestUserPassword(string userName)
        {
            // Generate a secure password
            string securePassword = GenerateSecurePassword();

            // Hash it
            string password = LoginHelper.GetHashPassword(securePassword);

            // Notify the user
            Log.Logger.Information("{0} has the new password: {1}", userName, securePassword);
            return password;
        }

        /// <summary>
        ///     Generates a secure password.
        /// </summary>
        /// <returns>The generated password.</returns>
        private static string GenerateSecurePassword()
        {
            const int requiredLength = 20;

            bool requireNonAlphanumeric = true;
            bool requireDigit = true;
            bool requireLowercase = true;
            bool requireUppercase = true;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while(password.Length < requiredLength)
            {
                char c = (char) random.Next(33, 126);

                password.Append(c);

                if(char.IsDigit(c))
                    requireDigit = false;
                else if(char.IsLower(c))
                    requireLowercase = false;
                else if(char.IsUpper(c))
                    requireUppercase = false;
                else if(!char.IsLetterOrDigit(c)) requireNonAlphanumeric = false;
            }

            if(requireNonAlphanumeric) password.Append((char) random.Next(33, 48));
            if(requireDigit) password.Append((char) random.Next(48, 58));
            if(requireLowercase) password.Append((char) random.Next(97, 123));
            if(requireUppercase) password.Append((char) random.Next(65, 91));

            return password.ToString();
        }

    }

}
