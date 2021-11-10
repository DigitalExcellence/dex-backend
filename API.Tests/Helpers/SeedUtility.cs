using API.HelperClasses;
using Data;
using Data.Helpers;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Tests.Helpers
{
    public class SeedUtility
    {
        public static void InitializeDbForTests(ApplicationDbContext context)
        {
            // Seed roles
            SeedHelper.InsertRoles(Seed.SeedRoles(), context);
            List<Role> roles = context.Role.ToList();

            // Seed institutions
            context.Institution.AddRange(Seed.SeedInstitution());
            context.SaveChanges();

            // Seed admin user
            context.User.Add(Seed.SeedAdminUser(roles));
            context.SaveChanges();

            // Seed random users
            context.User.Add(Seed.SeedPrUser(roles));
            context.User.AddRange(Seed.SeedUsers(roles));
            context.User.Add(Seed.SeedDataOfficerUser(roles));
            context.SaveChanges();

            // Seed projects
            List<User> users = context.User.ToList();
            context.Project.AddRange(Seed.SeedProjects(users));
            context.SaveChanges();

            // Seed collaborators
            List<Project> projectsForCollaborators = context.Project.ToList();
            context.Collaborators.AddRange(Seed.SeedCollaborators(projectsForCollaborators));
            context.SaveChanges();

            // Seed highlights
            List<Project> projectsForHighlights = context.Project.ToList();
            context.Highlight.AddRange(Seed.SeedHighlights(projectsForHighlights));
            context.SaveChanges();

            // Seed wizardpages
            context.WizardPage.AddRange(Seed.SeedWizardPages());
            context.SaveChanges();

            // Seed datasources
            context.DataSource.AddRange(Seed.SeedDataSources());
            context.SaveChanges();
            SeedHelper.SeedDataSourceWizardPages(context);
        }
    }
}
