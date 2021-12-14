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

        public static Category RandomCategory()
        {
            Faker<Category> categoryToFake = new Faker<Category>()
                                           .RuleFor(c => c.Name, f => f.Lorem.Word());
            Category category = categoryToFake.Generate();

            return category;
        }

        public static List<Tag> RandomTags()
        {
            List<Tag> tags = new List<Tag>();
            for(int i = 0; i < 3; i++)
            {
                Faker<Tag> tagToFake = new Faker<Tag>()
                    .RuleFor(t => t.Name, f => f.Hacker.Adjective());

                Tag tag = tagToFake.Generate();

                tags.Add(tag);
            }

            return tags;
        }

        public static Highlight RandomHighlight()
        {
            Faker<Highlight> highlightToFake = new Faker<Highlight>()
                                                 .RuleFor(s => s.Description, f => f.Lorem.Sentence(5))
                                                 .RuleFor(s => s.StartDate, DateTime.Now)
                                                 .RuleFor(s => s.EndDate, DateTime.Now.AddYears(2))
                                                 .RuleFor(s => s.Project, RandomProject());
            Highlight highlight = highlightToFake.Generate();

            return highlight;
        }
    }
}
