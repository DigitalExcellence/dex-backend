using Models;
using RestSharp;
using Services.Sources;
using System;
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
                return project.Likes.Count * Multiplier;
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
                return 3.00 * Multiplier;
            }
            return Multiplier / projectCreatedDays * 10;
        }
    }

    public class AverageLikeDateDataPoint : AbstractDataPoint
    {
        public AverageLikeDateDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            if(project.Likes.Count > 0)
            {
                DateTime averageDateTime = DateTime
                            .MinValue
                            .AddSeconds
                            (project.Likes
                                 .Sum(r => (r.Date - DateTime.MinValue).TotalSeconds)
                                     / project.Likes.Count);
                double averageLikeDate = Math.Round((DateTime.Now - averageDateTime).TotalDays, 2);
                if(averageLikeDate < 14)
                {
                    return 2 * Multiplier;
                }
                return Multiplier / averageLikeDate;
            }
            return 0;
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
                return 2 * Multiplier;
            }
            return Multiplier / updatedDate;
        }
    }
    public class InstitutionDataPoint : AbstractDataPoint
    {
        public InstitutionDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            return project.LinkedInstitutions.Count * Multiplier;
        }
    }

    public class ConnectedCollaboratorsDataPoint : AbstractDataPoint
    {
        public ConnectedCollaboratorsDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            if(project.Collaborators.Count > 5) return Math.Round(6 * Multiplier, 2);
            return project.Collaborators.Count * Multiplier;
        }
    }

    public class MetaDataDataPoint : AbstractDataPoint
    {
        public MetaDataDataPoint(double multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            double score = 0;
            if(project.Categories.Count >= 1) score += 1;
            if(project.CallToActions.Count >= 1 && project.CallToActions.Count <= 4) score += project.CallToActions.Count;
            if(project.CallToActions.Count >= 4 )score += 4;
            if(project.Images.Count >= 1) score += 2;
            if(project.Uri != null) score += 1;
            return score * Multiplier;
        }
    }
    //ToDo: Research Repo System.
    public class RepoScoreDataPoint : AbstractDataPoint
    {
        public RepoScoreDataPoint(double multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {

            if(string.IsNullOrWhiteSpace(project.Uri)) return 0;
            Uri uri = new Uri(project.Uri);
            IRestClientFactory restClientFactory = new RestClientFactory();
            IRestClient client = restClientFactory.Create(uri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if((int)response.StatusCode < 400)
            {
                return 2 * Multiplier;
            }
            return 0;
        }
    }
}
