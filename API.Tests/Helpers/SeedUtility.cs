using API.HelperClasses;
using Bogus;
using Data;
using System;
using Data.Helpers;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace API.Tests.Helpers
{
    public class SeedUtility
    {
        public static Project RandomProject()
        {
            Faker<Project> projectToFake = new Faker<Project>()
                                           .RuleFor(p => p.UserId, 1)
                                           .RuleFor(p => p.Uri, f => f.Internet.Url())
                                           .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                                           .RuleFor(p => p.Description, f => f.Lorem.Sentences(10))
                                           .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentences(1));
            Project project = projectToFake.Generate();
            project.Created = DateTime.Now.AddDays(-2);
            project.Updated = DateTime.Now;

            return project;
        }
    }
}
