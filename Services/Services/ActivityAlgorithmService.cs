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

        private readonly List<AbstractDataPoint> dataPoints
                            = new List<AbstractDataPoint>()
                          {
                              new LikeDataPoint(1),
                              new RecentCreatedDataPoint(0.1),
                              new AverageLikeDateDataPoint(1),
                              new UpdatedTimeDataPoint(1),
                              new InstitutionDataPoint(1)
                          };

        private readonly IProjectService projectService;
        public ActivityAlgorithmService(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public List<Project> CalculateAllProjects(IEnumerable<Project> projects)
        {
            foreach(Project project in projects)
            {
                double score = CalculateProjectActivityScore(project);
                SetProjectActivityScore(project, score);
            }
            projectService.Save();
            return projects.ToList();
        }

        public double CalculateProjectActivityScore(Project project)
        {
            return dataPoints.Sum(dataPoint => dataPoint.Calculate(project));
        }

        public void SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            projectService.Update(project);
            //TODO Elastic search implementation
        }
    }


}
