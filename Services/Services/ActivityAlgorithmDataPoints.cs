using Microsoft.Extensions.DependencyInjection;
using Models;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public abstract class AbstractDataPoint
    {
        protected readonly double Multiplier;

        public AbstractDataPoint(double multiplier = 1)
        {
            Multiplier = multiplier;
        }
        public abstract double Calculate(Project project);

    }

    public class LikeDataPoint : AbstractDataPoint
    {
        public LikeDataPoint(double multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            if(project.Likes != null)
                return Math.Round(project.Likes.Count * Multiplier, 2);
            return 0.00;
        }


    }
    public class RecentCreatedDataPoint : AbstractDataPoint
    {
        public RecentCreatedDataPoint(double multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            double projectCreatedDays = (DateTime.Now - project.Created).TotalDays;
            if(projectCreatedDays < 14)
            {
                return 5.00;
            }
            // This does not make sense, older == more points?
            return Math.Round(Multiplier / projectCreatedDays * 10, 2);
        }
    }

    public class AverageLikeDateDataPoint : AbstractDataPoint
    {
        public AverageLikeDateDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach(ProjectLike projectLike in project.Likes)
            {
                dates.Add(projectLike.Date);
            }
            if(dates.Count > 0)
            {
                DateTime averageDateTime = DateTime
                            .MinValue
                            .AddSeconds
                            (dates
                                 .Sum(r => (r - DateTime.MinValue).TotalSeconds)
                                     / dates.Count);
                double averageLikeDate = Math.Round((DateTime.Now - averageDateTime).TotalDays, 2);
                if(averageLikeDate < 14)
                {
                    return 2.00;
                }
                return Math.Round(Multiplier / averageLikeDate, 2);
            }
            return 0.00;
        }
    }

    public class UpdatedTimeDataPoint : AbstractDataPoint
    {
        public UpdatedTimeDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            double updatedDate = (DateTime.Now - project.Updated).TotalDays;
            if(updatedDate < 14)
            {
                return 2.00;
            }
            return Math.Round(Multiplier / updatedDate, 2);
        }
    }

    public class InstitutionDataPoint : AbstractDataPoint
    {
        public InstitutionDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            return Math.Round(project.LinkedInstitutions.Count * Multiplier, 2);
        }
    }

    public class ConnectedCollaboratorsDataPoint : AbstractDataPoint
    {
        public ConnectedCollaboratorsDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            if(project.Collaborators.Count > 5) return Math.Round(6 * Multiplier, 2);
            return Math.Round(project.Collaborators.Count * Multiplier, 2);
        }
    }

    public class MetaDataDataPoint : AbstractDataPoint
    {
        public MetaDataDataPoint(double multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            double score = 0;
            if(project.Categories.Count >= 1) score += 1;
            if(project.CallToActions.Count >= 1) score += project.CallToActions.Count;
            if(project.Images.Count >= 1) score += 2;
            if(project.Uri != null) score += 1;
            return Math.Round(score * Multiplier, 2);
        }
    }
    //ToDo: Research Repo System.
    public class RepoScoreDataPoint : AbstractDataPoint
    {
        readonly IGitLabSource gitLabSource = new GitLabSource(new RestClientFactory());
        public RepoScoreDataPoint(double multiplier = 1) : base(multiplier) {}
        public override double Calculate(Project project)
        {
            Uri sourceUri = new Uri(project.Uri);

            if(sourceUri.Host == "github.com")
            {
                return 2.00 * Multiplier;
            }
            if(sourceUri.Host == "gitlab.com")
            {
                Project gitlabProject = gitLabSource.GetProjectInformation(sourceUri);
                if(gitlabProject != null)
                {
                    return Math.Round(2.00 * Multiplier, 2);
                } else
                {
                    return 0.00;
                }
            }
            return 2.00 * Multiplier;
        }
    }
}
