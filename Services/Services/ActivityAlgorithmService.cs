using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IActivityAlgorithmService
    {
        IEnumerable<Project> CalculateAllProjects(IEnumerable<Project> projects);
        double CalculateProjectActivityScore(Project project);
        bool SetProjectActivityScore(Project project, double score);

    }
    public class ActivityAlgorithmService : IActivityAlgorithmService
    {

        private readonly List<IActivityAlgorithmDataPoint> dataPoints;
        private readonly ProjectRepository projectRepo;

        public ActivityAlgorithmService(ProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;
            //List all data points wat are required to calculate each projects activity score
            dataPoints = new List<IActivityAlgorithmDataPoint>()
                          {
                              new LikeDataPoint()
                          };
        }

        public IEnumerable<Project> CalculateAllProjects(IEnumerable<Project> projects)
        {
            foreach(Project project in projects.ToList())
            {
                double score = this.CalculateProjectActivityScore(project);
                this.SetProjectActivityScore(project, score);
            }
            return projects;
        }

        public double CalculateProjectActivityScore(Project project)
        {
            double score = 0;
            foreach(IActivityAlgorithmDataPoint dataPoint in this.dataPoints)
            {
                score += dataPoint.Calculate(project);
            }
            return score;

            //Is this better?
            // return this._dataPoints.Sum(dataPoint => dataPoint.Calculate(project));
        }

        public bool SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            //TODO database implementation
            //TODO Elastic search implementation
            return true;
        }
    }

    internal interface IActivityAlgorithmDataPoint
    {

        public double Calculate(Project project);

    }


    internal class  LikeDataPoint : IActivityAlgorithmDataPoint
    {

        public double Calculate(Project project)
        {
            return project.Likes.Count;
        }

    }
}
