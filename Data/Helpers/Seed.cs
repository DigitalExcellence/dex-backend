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

using Bogus;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Defaults;
using System;
using System.Collections.Generic;

namespace Data.Helpers
{

    /// <summary>
    ///     Class for helpers to seed data into the database
    /// </summary>
    public static class Seed
    {
        /// <summary>
        ///     Seed random users into the database using fake date from Bogus
        /// </summary>
        public static List<User> SeedUsers(this ModelBuilder modelBuilder)
        {
            List<User> users = new List<User>();
            for(int i = 0; i < 30; i++)
            {
                Faker<User> userToFake = new Faker<User>()
                                         .RuleFor(s => s.Id, i + 1)
                                         .RuleFor(s => s.Name, f => f.Name.FirstName())
                                         .RuleFor(s => s.Email, f => f.Internet.Email());

                User user = userToFake.Generate();

                user.IdentityId = (i + 2).ToString();
                modelBuilder.Entity<User>()
                            .HasData(user);
                users.Add(user);
            }
            return users;
        }
        public static void SeedRoles(this ModelBuilder modelBuilder, List<User> users)
        {
            modelBuilder.Entity<Role>(b =>
            {
                b.HasData(new Role
                {
                    Id = 1,
                    Name = "RegisteredUser",

                });
            });
            modelBuilder.Entity<RoleScope>(b =>
                b.HasData(new
                {
                    RoleId = 1,
                    Id = 1,
                    Scope = nameof(Defaults.Scopes.ProjectWrite)
                },
                new
                {
                    RoleId = 1,
                    Id = 2,
                    Scope = nameof(Defaults.Scopes.UserWrite)
                }));

            modelBuilder.Entity<Role>(b =>
            {
                b.HasData(new Role
                {
                    Id = 2,
                    Name = "PR",

                });
            });
            modelBuilder.Entity<RoleScope>(b =>

                b.HasData(new
                {
                    RoleId = 2,
                    Id = 3,
                    Scope = nameof(Defaults.Scopes.HighlightWrite)
                }));

            modelBuilder.Entity<Role>(b =>
            {
                b.HasData(new Role
                {
                    Id = 3,
                    Name = "Administrator",

                });
            });
            modelBuilder.Entity<RoleScope>(b =>
                b.HasData(
                new {
                    RoleId = 3,
                    Id = 4,
                    Scope = nameof(Defaults.Scopes.ProjectWrite)
                },
                new
                {
                    RoleId = 3,
                    Id = 5,
                    Scope = nameof(Defaults.Scopes.UserWrite)
                },
                new
                {
                    RoleId = 3,
                    Id = 6,
                    Scope = nameof(Defaults.Scopes.UserRead)
                },
                new
                {
                    RoleId = 3,
                    Id = 7,
                    Scope = nameof(Defaults.Scopes.RoleRead)
                },
                new
                {
                    RoleId = 3,
                    Id = 8,
                    Scope = nameof(Defaults.Scopes.RoleWrite)
                },
                new
                {
                    RoleId = 3,
                    Id = 9,
                    Scope = nameof(Defaults.Scopes.HighlightWrite)
                }));

            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 31,
                    IdentityId = "1",
                    Name = "Regular User",
                    Email = "a@b.c",
                    RoleId = 1
                },
                new
                {
                    Id = 32,
                    IdentityId = "2",
                    Name = "PR User",
                    Email = "a@b.c",
                    RoleId = 2
                },
                new
                {
                    Id = 33,
                    IdentityId = "3",
                    Name = "Administrator",
                    Email = "a@b.c",
                    RoleId = 3
                }
            );
        }


        /// <summary>
        ///     Seed random projects into the database using fake date from Bogus
        /// </summary>
        public static List<Project> SeedProjects(this ModelBuilder modelBuilder, List<User> users)
        {
            if(users.Count < 1) return null;
            List<Project> projects = new List<Project>();
            Random r = new Random();
            for(int i = 0; i < 30; i++)
            {
                User user = users[r.Next(0, users.Count - 1)];
                Faker<Project> projectToFake = new Faker<Project>()
                                               .RuleFor(s => s.Id, i + 1)
                                               .RuleFor(s => s.UserId, user.Id)
                                               .RuleFor(s => s.Uri, f => f.Internet.Url())
                                               .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                                               .RuleFor(s => s.Description, f => f.Lorem.Sentences(10))
                                               .RuleFor(s => s.ShortDescription, f => f.Lorem.Sentences(1));
                Project project = projectToFake.Generate();

                project.Created = DateTime.Now.AddDays(-2);
                project.Updated = DateTime.Now;
                projects.Add(project);
                modelBuilder.Entity<Project>()
                            .HasData(project);
            }

            return projects;
        }

        /// <summary>
        ///     Seed random Collaborators into the database using fake date from Bogus
        /// </summary>
        public static void SeedCollaborators(this ModelBuilder modelBuilder, List<Project> projects)
        {
            foreach(Project project in projects)
            {
                Faker<Collaborator> collaboratorToFake = new Faker<Collaborator>()
                                                         .RuleFor(s => s.Id, f => f.Random.Number(1, 9999))
                                                         .RuleFor(c => c.FullName, f => f.Name.FullName())
                                                         .RuleFor(c => c.Role, f => f.Name.JobTitle());

                Collaborator collaborator = collaboratorToFake.Generate();
                Collaborator collaborator2 = collaboratorToFake.Generate();
                collaborator.ProjectId = project.Id;
                collaborator2.ProjectId = project.Id;
                modelBuilder.Entity<Collaborator>()
                            .HasData(collaborator, collaborator2);
            }
        }

    }

}
