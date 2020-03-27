using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Helpers
{
    /// <summary>
    /// Class for helpers to seed data into the database
    /// </summary>
    public static class Seed
    {

        /// <summary>
        /// Seed random users into the database using fake date from Bogus
        /// </summary>
        public static List<User> SeedUsers(this ModelBuilder modelBuilder)
        {
            List<User> users = new List<User>();
            for (var i = 0; i < 30; i++)
            {
                var userToFake = new Faker<User>()
                    .RuleFor(s => s.Id, i+1)
                    .RuleFor(s => s.Name, f => f.Name.FirstName())
                    .RuleFor(s => s.Email, f => f.Internet.Email());

                var user = userToFake.Generate();

                user.IdentityId = (i + 2).ToString();
                modelBuilder.Entity<User>().HasData(user);
                users.Add(user);
            }
            return users;
        }

        /// <summary>
        /// Seed random projects into the database using fake date from Bogus
        /// </summary>
        public static void SeedProjects(this ModelBuilder modelBuilder, List<User> users)
        {
            if (users.Count < 1) return;

            Random r = new Random();
            for (var i = 0; i < 30; i++)
            {
                var user = users[r.Next(0, users.Count - 1)];

                var projectToFake = new Faker<Project>()
                    .RuleFor(s => s.Id, i+1)
                    .RuleFor(s => s.UserId, user.Id)
                    .RuleFor(s => s.Uri, f => f.Internet.Url())
                    .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                    .RuleFor(s => s.Description, f => f.Lorem.Letter(150))
                    .RuleFor(s => s.ShortDescription, f => f.Lorem.Letter(30));
                var project = projectToFake.Generate();

                project.Created = DateTime.Now.AddDays(-2);
                project.Updated = DateTime.Now;
                modelBuilder.Entity<Project>().HasData(project);
            }
        }
    }
}