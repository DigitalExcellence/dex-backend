using Models;
using System;

namespace Services.Services
{
    public abstract class AbstractDataPoint
    {
        protected readonly double Multiplier;

        public AbstractDataPoint(int multiplier = 1)
        {
            Multiplier = multiplier;
        }
        public abstract double Calculate(Project project);

    }

    public class LikeDataPoint : AbstractDataPoint
    {
        public LikeDataPoint(int multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            if(project.Likes != null)
                return project.Likes.Count * Multiplier;
            return 0;
        }


    }
    public class RecentCreatedDataPoint : AbstractDataPoint
    {
        public RecentCreatedDataPoint(int multiplier = 1) : base(multiplier) { }
        public override double Calculate(Project project)
        {
            return (DateTime.Now - project.Created).TotalDays * Multiplier;
        }
    }

    // TODO: Update like DataPoint
    // TODO: Updated Time
    // TODO: Average Like Date
    // TODO: Institution
    // TODO: Connected Collaborators
    // TODO: Sufficient MetaData
    // TODO: CTA'S
    // TODO: Research and implementation for GitHub
}
