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
        List<Project> CalculateAllProjects(IEnumerable<Project> projects);
        double CalculateProjectActivityScore(Project project);
        void SetProjectActivityScore(Project project, double score);

    }
    public class ActivityAlgorithmService : IActivityAlgorithmService
    {

        private readonly List<IActivityAlgorithmDataPointService> dataPoints
                            = new List<IActivityAlgorithmDataPointService>()
                          {
                              new LikeDataPoint(),
                              new RecentCreationDate(),
                          };
        private readonly IProjectRepository projectRepository;
        private readonly IProjectService projectService;
        public ActivityAlgorithmService(IProjectRepository projectRepository, IProjectService projectService)
        {
            this.projectRepository = projectRepository;
            this.projectService = projectService;
            //List all data points wat are required to calculate each projects activity score

        }

        public List<Project> CalculateAllProjects(IEnumerable<Project> projects)
        {
            foreach(Project project in projects.ToList())
            {
                double score = CalculateProjectActivityScore(project);
                SetProjectActivityScore(project, score);
            }
            projectService.Save();
            return projects.ToList();
        }

        public double CalculateProjectActivityScore(Project project)
        {
            double score = 0;
            foreach(IActivityAlgorithmDataPointService dataPoint in dataPoints)
            {
                score += dataPoint.Calculate(project);
            }
            return score;

            //Is this better?
            // return this._dataPoints.Sum(dataPoint => dataPoint.Calculate(project));
        }

        public void SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            projectService.Update(project);


            //TODO database implementation
            //TODO Elastic search implementation
            return;
        }
    }


}
