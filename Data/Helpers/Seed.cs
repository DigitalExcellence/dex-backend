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
using Models;
using Models.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Helpers
{

    /// <summary>
    ///     Class for helpers to seed data into the database
    /// </summary>
    public static class Seed
    {

        /// <summary>
        ///     Temporary method which is meant to update the rolescopes in staging / production after user graduation feature is
        ///     implemented.
        /// </summary>
        /// <returns>List of new rolescopes.</returns>
        public static List<RoleScope> UpdateRoleScopes()
        {
            List<RoleScope> roleScopes = new List<RoleScope>();
            for(int i = 1; i < 5; i++)
            {
                RoleScope roleScope = new RoleScope("ProjectWrite", i)
                {
                    RoleId = i
                };
                roleScopes.Add(roleScope);
            }

            RoleScope adminRoleScope = new RoleScope("AdminProjectWrite", 4);
            roleScopes.Add(adminRoleScope);

            return roleScopes;
        }

        /// <summary>
        ///     Temporary method which is meant to add the alumni role in staging / production after user graduation feature is
        ///     implemented.
        /// </summary>
        /// <returns>Alumni role.</returns>
        public static Role SeedAlumniRole()
        {
            Role alumniRole = new Role
            {
                Name = Defaults.Roles.Alumni
            };

            return alumniRole;
        }


        /// <summary>
        ///     Seed random users into the database using fake date from Bogus
        /// </summary>
        public static List<User> SeedUsers(List<Role> roles)
        {
            Role registeredUserRole = roles.Find(i => i.Name == nameof(Defaults.Roles.RegisteredUser));
            List<User> users = new List<User>();
            for(int i = 0; i < 30; i++)
            {
                Faker<User> userToFake = new Faker<User>()
                                         .RuleFor(s => s.Name, f => f.Name.FirstName())
                                         .RuleFor(s => s.Email, f => f.Internet.Email());

                User user = userToFake.Generate();
                user.Role = registeredUserRole;
                user.IdentityId = (i + 2).ToString();

                users.Add(user);
            }
            return users;
        }

        /// <summary>
        ///     Seeds the roles.
        /// </summary>
        /// <returns>The list of roles that will be seeded.</returns>
        public static List<Role> SeedRoles()
        {
            List<Role> roles = new List<Role>();
            Role registeredUserRole = new Role
            {
                Name = nameof(Defaults.Roles.RegisteredUser),

                Scopes = new List<RoleScope>
                                                   {
                                                       new RoleScope(nameof(Defaults.Scopes.ProjectWrite))
                                                   }
            };
            roles.Add(registeredUserRole);

            Role prRole = new Role
            {
                Name = nameof(Defaults.Roles.PrUser),
                Scopes = new List<RoleScope>
                                       {
                                           new RoleScope(nameof(Defaults.Scopes.EmbedRead)),
                                           new RoleScope(nameof(Defaults.Scopes.EmbedWrite)),
                                           new RoleScope(nameof(Defaults.Scopes.HighlightRead)),
                                           new RoleScope(nameof(Defaults.Scopes.HighlightWrite)),
                                           new RoleScope(nameof(Defaults.Scopes.ProjectWrite))
                                       }
            };
            roles.Add(prRole);

            Role dataOfficerRole = new Role
            {
                Name = nameof(Defaults.Roles.DataOfficer),
                Scopes = new List<RoleScope>
                                                {
                                                    new RoleScope(nameof(Defaults.Scopes.InstitutionUserRead)),
                                                    new RoleScope(nameof(Defaults.Scopes.InstitutionUserWrite)),
                                                    new RoleScope(nameof(Defaults.Scopes.InstitutionEmbedWrite)),
                                                    new RoleScope(nameof(Defaults.Scopes.InstitutionProjectWrite)),
                                                    new RoleScope(nameof(Defaults.Scopes.ProjectWrite))
                                                }
            };
            roles.Add(dataOfficerRole);

            Role administratorRole = new Role
            {
                Name = nameof(Defaults.Roles.Administrator),
                Scopes = new List<RoleScope>
                                                  {
                                                      new RoleScope(nameof(Defaults.Scopes.AdminProjectWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.UserWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.UserRead)),
                                                      new RoleScope(nameof(Defaults.Scopes.RoleRead)),
                                                      new RoleScope(nameof(Defaults.Scopes.RoleWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.HighlightRead)),
                                                      new RoleScope(nameof(Defaults.Scopes.HighlightWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.EmbedRead)),
                                                      new RoleScope(nameof(Defaults.Scopes.EmbedWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.InstitutionRead)),
                                                      new RoleScope(nameof(Defaults.Scopes.InstitutionWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.FileWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.CallToActionOptionWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.ProjectWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.DataSourceWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.WizardPageWrite)),
                                                      new RoleScope(nameof(Defaults.Scopes.AdminProjectExport))
                                                  }
            };
            roles.Add(administratorRole);

            Role alumniRole = new Role
            {
                Name = nameof(Defaults.Roles.Alumni),
                Scopes = new List<RoleScope>()
            };
            roles.Add(alumniRole);

            return roles;
        }

        /// <summary>
        ///     Seeds the admin user.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Returns the admin user that will be seeded.</returns>
        public static User SeedAdminUser(List<Role> roles)
        {
            Role adminRole = roles.Find(i => i.Name == nameof(Defaults.Roles.Administrator));

            User user = new User
            {
                Role = adminRole,
                IdentityId = "88421113",
                Email = "Administrator@dex.software",
                Name = "Administrator bob",
            };

            return user;
        }

        /// <summary>
        ///     This method seeds a test institution in the database.
        /// </summary>
        /// <returns>Returns the institution that will be seeded in the database.</returns>
        public static Institution SeedInstitution()
        {
            Institution institution = new Institution
            {
                Name = "Fontys",
                Description = "Description for Fontys",
                IdentityId = "https://identity.fhict.nl"
            };
            return institution;
        }

        public static List<Institution> SeedInstitutions()
        {
            return new List<Institution>
            {
                new Institution
                {
                    Name = "Fontys",
                    Description = "Description for Fontys",
                    IdentityId = "https://identity.fhict.nl"
                },
                new Institution()
                {
                    Name = "Fontys2",
                    Description = "Description for Fontys2",
                    IdentityId = "https://identity.fhict.nl"
                },
                new Institution()
                {
                    Name = "Fontys3",
                    Description = "Description for Fontys3",
                    IdentityId = "https://identity.fhict.nl"
                }
            };
        }

        public static IEnumerable<ProjectLike> SeedProjectLikes(List<Project> projects)
        {
            foreach(Project project in projects)
            {
                yield return new ProjectLike { Date = DateTime.Now, LikedProject = project, UserId = 1 };
            }
        }

        /// <summary>
        ///     Seeds the pr user.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns>Returns the PR user that will be seeded.</returns>
        public static User SeedPrUser(List<Role> roles)
        {
            Role prRole = roles.Find(i => i.Name == nameof(Defaults.Roles.PrUser));
            User user = new User
            {
                IdentityId = "985632147",
                Email = "Pr@dex.software",
                Name = "Pr jerry",
                Role = prRole
            };

            return user;
        }

        /// <summary>
        ///     This method seeds the data officer user.
        /// </summary>
        /// <param name="roles">This variable contains the roles that exist.</param>
        /// <returns>This method returns the data officer role that will be seeded.</returns>
        public static User SeedDataOfficerUser(List<Role> roles)
        {
            Role dataOfficerRole = roles.Find(role => role.Name == nameof(Defaults.Roles.DataOfficer));
            User user = new User
            {
                IdentityId = "954654861",
                Email = "dataofficer@dex.software",
                Name = "data officer Sam",
                Role = dataOfficerRole,
                InstitutionId = 1
            };

            return user;
        }

        public static User SeedAlumniUser(List<Role> roles)
        {
            Role alumniRole = roles.Find(i => i.Name == nameof(Defaults.Roles.Alumni));
            User user = new User
            {
                IdentityId = "123456789",
                Email = "Alumni@dex.software",
                Name = "Alumni test",
                Role = alumniRole
            };

            return user;
        }

        /// <summary>
        ///     Seed random projects into the database using fake date from Bogus
        /// </summary>
        public static List<Project> SeedProjects(List<User> users)
        {
            if(users.Count < 1) return null;
            List<Project> projects = new List<Project>();
            Random r = new Random();
            for(int i = 0; i < 30; i++)
            {
                User user = users[r.Next(0, users.Count - 1)];
                Faker<Project> projectToFake = new Faker<Project>()
                                               .RuleFor(s => s.UserId, user.Id)
                                               .RuleFor(s => s.Uri, f => f.Internet.Url())
                                               .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                                               .RuleFor(s => s.Description, f => f.Lorem.Sentences(10))
                                               .RuleFor(s => s.ShortDescription, f => f.Lorem.Sentences(1));
                Project project = projectToFake.Generate();

                project.Created = DateTime.Now.AddDays(-2);
                project.Updated = DateTime.Now;

                projects.Add(project);
            }

            return projects;
        }

        /// <summary>
        ///     Seed random Collaborators into the database using fake date from Bogus
        /// </summary>
        public static List<Collaborator> SeedCollaborators(List<Project> projects)
        {
            List<Collaborator> collaborators = new List<Collaborator>();
            foreach(Project project in projects)
            {
                Faker<Collaborator> collaboratorToFake = new Faker<Collaborator>()
                                                         .RuleFor(c => c.FullName, f => f.Name.FullName())
                                                         .RuleFor(c => c.Role, f => f.Name.JobTitle());

                Collaborator collaborator = collaboratorToFake.Generate();
                Collaborator collaborator2 = collaboratorToFake.Generate();
                collaborator.ProjectId = project.Id;
                collaborator2.ProjectId = project.Id;


                collaborators.Add(collaborator);
                collaborators.Add(collaborator2);
            }
            return collaborators;
        }

        /// <summary>
        /// Seed random ProjectLikes into the database
        /// </summary>
        public static List<ProjectLike> SeedLikes(List<Project> projects, List<User> users)
        {
            List<ProjectLike> projectLikes = new List<ProjectLike>();
            foreach(Project project in projects)
            {
                List<User> usersThatLiked = new List<User>();
                Random random = new Random();
                int randomLikes = random.Next(5, 15);
                for(int i = 0; i < randomLikes; i++)
                {
                    ProjectLike projectLike = new ProjectLike
                    {
                        UserId = project.User.Id,
                        LikedProject = project
                    };

                    bool userFound = false;
                    while(!userFound)
                    {
                        int randomUserId = random.Next(0, users.Count);
                        User u = users[randomUserId];
                        if(!usersThatLiked.Contains(u))
                        {
                            projectLike.ProjectLiker = u;
                            usersThatLiked.Add(u);
                            userFound = true;
                        }
                    }

                    projectLikes.Add(projectLike);
                }

            }
            return projectLikes;
        }

        /// <summary>
        ///     Seeds the highlights.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns>Returns a list of project highlights that wil be seeded.</returns>
        public static List<Highlight> SeedHighlights(List<Project> projects)
        {
            List<Highlight> highlights = new List<Highlight>();

            for(int i = 0; i < 5 && i < projects.Count; i++)
            {
                Faker<Highlight> fakeHighLight = new Faker<Highlight>()
                                                 .RuleFor(s => s.Description, f => f.Lorem.Sentence(5))
                                                 .RuleFor(s => s.StartDate, DateTime.Now)
                                                 .RuleFor(s => s.EndDate, DateTime.Now.AddYears(2))
                                                 .RuleFor(s => s.Project, projects[i]);

                Highlight highlight = fakeHighLight.Generate();

                highlights.Add(highlight);
            }

            return highlights;
        }

        /// <summary>
        ///     This method seeds call to action options.
        /// </summary>
        /// <returns>Returns a list of call to actions options that can be seeded into the database.</returns>
        public static List<CallToActionOption> SeedCallToActionOptions()
        {
            return new List<CallToActionOption>
                   {
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "Join today"
                       },
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "Provide feedback"
                       },
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "Apply now"
                       },
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "Collaborate with us"
                       },
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "More information"
                       },
                       new CallToActionOption
                       {
                           Type = "title",
                           Value = "Get in touch"
                       }
                   };
        }

        /// <summary>
        ///     This method seeds wizard pages.
        /// </summary>
        /// <returns>Returns a list of wizard pages that can be seeded into the database.</returns>
        public static List<WizardPage> SeedWizardPages()
        {
            return new List<WizardPage>
                   {
                       new WizardPage
                       {
                           Name = "Enter your username",
                           Description = "Enter the username you would like to retrieve projects from"
                       },
                       new WizardPage
                       {
                           Name = "Paste the link to your project",
                           Description = "Enter the link to your project that you would like to import"
                       },
                       new WizardPage
                       {
                           Name = "Select the correct project",
                           Description = "Select which project you would like to retrieve"
                       }
                   };
        }

        public static List<DataSource> SeedDataSources()
        {
            return new List<DataSource>
                   {
                       new DataSource
                       {
                           Title = "Github",
                           Guid = "de38e528-1d6d-40e7-83b9-4334c51c19be",
                           IsVisible = true,
                           Description = "Seeded description for the Github data source adaptee"
                       },
                       new DataSource
                       {
                           Title = "Gitlab",
                           Guid = "66de59d4-5db0-4bf8-a9a5-06abe8d3443a",
                           IsVisible = true,
                           Description = "Seeded description for the Gitlab data source adaptee"
                       },
                       new DataSource
                       {
                           Title = "JsFiddle",
                           Guid = "96666870-3afe-44e2-8d62-337d49cf972d",
                           IsVisible = false,
                           Description = "Seeded description for the JsFiddle data source adaptee"
                       }
                   };
        }

        public static User SeedAdminUser2(List<Role> roles)
        {
            Role adminRole = roles.Find(i => i.Name == nameof(Defaults.Roles.Administrator));

            User user = new User
            {
                Role = adminRole,
                IdentityId = "32423446",
                Email = "elastic_admin@dex.software",
                Name = "ElasticSearch Admin",
            };

            return user;
        }
    }

}
