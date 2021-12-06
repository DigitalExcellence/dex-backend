using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public abstract interface IActivityAlgorithmDataPointService
    {
        public IActivityAlgorithmDataPointService(int multiplier);
        public double Calculate(Project project);
    }

    public class LikeDataPoint : IActivityAlgorithmDataPointService
    {

        public double Calculate(Project project)
        {
            if(project.Likes != null)
                return project.Likes.Count;
            return 0;
        }

    }

    public class RecentCreationDate : IActivityAlgorithmDataPointService
    {
        public double Calculate(Project project)
        {
            if(project.Created != null)
            {
                if((DateTime.Now - project.Created).TotalDays < 30)
                {
                    return 10;
                }

            }
            return 0;
        }
    }

    // TODO: Update like DataPoint
    // TODO: Updated Time
    // TODO: Average Like Date
    // TODO: Institution
    // TODO: Connectec Collaborators
    // TODO: Sufficient MetaData
    // TODO: CTA'S
    // TODO: Research and implementation for GitHub
}
