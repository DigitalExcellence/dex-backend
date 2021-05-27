using API.HelperClasses;
using Data;
using Data.Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrationTests
{
    public class SeedUtility
    {
        public static void InitializeDbForTests(ApplicationDbContext context)
        {
            SeedHelper.InsertRoles(Seed.SeedRoles(), context);
            List<Role> roles = context.Role.ToList();

            context.Institution.AddRange(Seed.SeedInstitution());
            context.SaveChanges();

            context.User.Add(Seed.SeedAdminUser(roles));
            context.SaveChanges();

            //Seed random users
            context.User.Add(Seed.SeedPrUser(roles));
            context.User.AddRange(Seed.SeedUsers(roles));
            context.User.Add(Seed.SeedDataOfficerUser(roles));
            context.SaveChanges();

            //Seed projects
            List<User> users = context.User.ToList();
            context.Project.AddRange(Seed.SeedProjects(users));
            context.SaveChanges();

            //seed collaborators
            List<Project> projectsForCollaborators = context.Project.ToList();
            context.Collaborators.AddRange(Seed.SeedCollaborators(projectsForCollaborators));
            context.SaveChanges();

            List<Project> projectsForHighlights = context.Project.ToList();
            context.Highlight.AddRange(Seed.SeedHighlights(projectsForHighlights));
            context.SaveChanges();

            context.WizardPage.AddRange(Seed.SeedWizardPages());
            context.SaveChanges();

            context.DataSource.AddRange(Seed.SeedDataSources());
            context.SaveChanges();
            SeedHelper.SeedDataSourceWizardPages(context);
        }
    }
}
