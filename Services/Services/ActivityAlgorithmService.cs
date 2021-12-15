using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Repositories;
using System.Linq;
using System.Threading.Tasks;
using Services.Base;

namespace Services.Services
{
    public interface IActivityAlgorithmService
    {
        List<Project> CalculateAllProjects(IEnumerable<Project> projects);
        double CalculateProjectActivityScore(Project project, List<AbstractDataPoint> dataPoints);
        void SetProjectActivityScore(Project project, double score);
        ProjectActivityConfig GetActivityAlgorithmMultiplier();
        void SetActivityAlgorithmMultiplier(ProjectActivityConfig projectActivityConfig);

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

        private readonly IProjectService projectService;
        private readonly IActivityAlgorithmRepository activityAlgorithmRepository;

        /// <summary>
        /// Constructor for the activity algorithm service
        /// </summary>
        /// <param name="activityAlgorithmRepository">Initializes the activity algorithm repository</param>
        /// <param name="projectService">Initializes the project service</param>
        public ActivityAlgorithmService(IActivityAlgorithmRepository activityAlgorithmRepository, IProjectService projectService)
        {
            this.projectService = projectService;
            this.activityAlgorithmRepository = activityAlgorithmRepository;
        }
        
        /// <summary>
        /// Calculate all the projects
        /// </summary>
        /// <param name="projects">Update list of projects.</param>
        /// <returns>A list of projects</returns>
        public List<Project> CalculateAllProjects(IEnumerable<Project> projects)
        {
            ProjectActivityConfig multiplier = GetActivityAlgorithmMultiplier();
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
            foreach(Project project in projects)
            {
                double score = CalculateProjectActivityScore(project);
                SetProjectActivityScore(project, score);
            }
            projectService.Save();
            return projects.ToList();
        }
        /// <summary>
        /// Calculate the score per project
        /// </summary>
        /// <param name="project">The project that has to be calculated</param>
        /// <param name="dataPoints">The datapoints whether it should look at.</param>
        /// <returns>the project score</returns>
        public double CalculateProjectActivityScore(Project project, List<AbstractDataPoint> dataPoints = null)
        {
            if(dataPoints == null)
                dataPoints = this.dataPoints;
            return Math.Round(dataPoints.Sum(dataPoint => dataPoint.Calculate(project)), 2);
        }
        /// <summary>
        /// Sets the project score
        /// </summary>
        /// <param name="project">The project which score needs to be set.</param>
        /// <param name="score">The score of the project</param>
        public void SetProjectActivityScore(Project project, double score)
        {
            project.ActivityScore = score;
            projectService.UpdateActivityScore(project);
        }
        /// <summary>
        ///  Updates the project activity config.
        /// </summary>
        /// <param name="projectActivityConfig">The new config</param>
        public void SetActivityAlgorithmMultiplier(ProjectActivityConfig projectActivityConfig)
        {
            activityAlgorithmRepository.UpdateActivityAlgorithmConfig(projectActivityConfig);
            
        }
        /// <summary>
        /// Gets the activity algorithm multiplier
        /// </summary>
        /// <returns>Project activity configuration.</returns>
        public ProjectActivityConfig GetActivityAlgorithmMultiplier()
        {
            ProjectActivityConfig projectActivityConfig = activityAlgorithmRepository.GetActivityAlgorithmConfig().Result;
            return projectActivityConfig;
        }
    }
}
