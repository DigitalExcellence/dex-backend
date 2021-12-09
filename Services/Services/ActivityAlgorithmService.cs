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
        double CalculateProjectActivityScore(Project project, List<AbstractDataPoint> dataPoints);
        void SetProjectActivityScore(Project project, double score);

    }
    public class ActivityAlgorithmService : IActivityAlgorithmService
    {

        private readonly List<AbstractDataPoint> _dataPoints
                            = new List<AbstractDataPoint>()
                          {
                              new LikeDataPoint(1),
                              new RecentCreatedDataPoint(1),
                              new AverageLikeDateDataPoint(1),
                              new UpdatedTimeDataPoint(1),
                              new InstitutionDataPoint(1),
                              new ConnectedCollaboratorsDataPoint(1),
                              new MetaDataDataPoint(1),
                              new RepoScoreDataPoint(1),
                          };

        private readonly IProjectService projectService;
        public ActivityAlgorithmService()
        {
            // this.projectService = projectService;
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

        public double CalculateProjectActivityScore(Project project, List<AbstractDataPoint> dataPoints = null)
        {
            if(dataPoints == null)
                dataPoints = _dataPoints;
            return Math.Round(dataPoints.Sum(dataPoint => dataPoint.Calculate(project)), 2);
        }

        public void SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            // projectService.Update(project);
            //TODO Elastic search implementation
        }
    }


}
