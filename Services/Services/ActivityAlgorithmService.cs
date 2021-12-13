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
        ActivityAlgorithmMultiplier GetActivityAlgorithmMultiplier();
        void SetActivityAlgorithmMultiplier(ActivityAlgorithmMultiplier activityAlgorithmMultiplier);

    }
    public class ActivityAlgorithmService : IActivityAlgorithmService
    {

        private List<AbstractDataPoint> dataPoints
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

        private readonly IProjectService? projectService;
        private readonly IActivityAlgorithmRepository activityAlgorithmRepository;
        public ActivityAlgorithmService(IActivityAlgorithmRepository activityAlgorithmRepository, IProjectService projectService)
        {
            this.projectService = projectService;
            this.activityAlgorithmRepository = activityAlgorithmRepository;
            ActivityAlgorithmMultiplier multiplier = GetActivityAlgorithmMultiplier();
            dataPoints = new List<AbstractDataPoint>()
            {
                new LikeDataPoint(multiplier.LikeDataMultiplier),
                new RecentCreatedDataPoint(multiplier.RecentCreatedDataMultiplier),
                new AverageLikeDateDataPoint(multiplier.AverageLikeDateMultiplier),
                new UpdatedTimeDataPoint(multiplier.UpdatedTimeMultiplier),
                new InstitutionDataPoint(multiplier.InstitutionMultiplier),
                new ConnectedCollaboratorsDataPoint(multiplier.ConnectedCollaboratorsMultiplier),
                new MetaDataDataPoint(multiplier.MetaDataMultiplier),
                new RepoScoreDataPoint(multiplier.RepoScoreMultiplier),
            };
        }
        public ActivityAlgorithmService(IProjectService? projectService = null)
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
            projectService?.Save();
            return projects.ToList();
        }

        public double CalculateProjectActivityScore(Project project, List<AbstractDataPoint> dataPoints = null)
        {
            if(dataPoints == null)
                dataPoints = this.dataPoints;
            return Math.Round(dataPoints.Sum(dataPoint => dataPoint.Calculate(project)), 2);
        }

        public void SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            projectService?.UpdateActivityScore(project);
        }
        public void SetActivityAlgorithmMultiplier(ActivityAlgorithmMultiplier activityAlgorithmMultiplier)
        {
            activityAlgorithmRepository.UpdateActivityAlgorithmMultiplierAsync(activityAlgorithmMultiplier);
        }
        public ActivityAlgorithmMultiplier GetActivityAlgorithmMultiplier()
        {
            ActivityAlgorithmMultiplier activityAlgorithmMultiplier = activityAlgorithmRepository.GetActivityAlgorithmMultiplierAsync().Result;
            return activityAlgorithmMultiplier;
        }
    }
}
