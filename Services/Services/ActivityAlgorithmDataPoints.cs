using Models;
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
            return 0;
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
                return 5;
            }
            // This does not make sense, older == more points?
            return Math.Round(projectCreatedDays * Multiplier, 2);
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
                double totalDays = Math.Round((DateTime.Now - averageDateTime).TotalDays, 2);
                if(totalDays < 14)
                {
                    return 2;
                }
                // Older equals more?
                return totalDays * Multiplier;
            }
            return 0;
        }
    }

    public class UpdatedTimeDataPoint : AbstractDataPoint
    {
        public UpdatedTimeDataPoint(double multiplier = 1) : base(multiplier) { }

        public override double Calculate(Project project)
        {
            return Math.Round((DateTime.Now - project.Updated).TotalDays * Multiplier, 2);
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
            return Math.Round(project.Collaborators.Count * Multiplier, 2);
        }
    }
    // TODO: Update like DataPoint
    // TODO: Updated Time
    // TODO: Institution
    // TODO: Connected Collaborators
    // TODO: Sufficient MetaData
    // TODO: CTA'S
    // TODO: Research and implementation for GitHub
}
