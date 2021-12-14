using FluentAssertions;
using Models;
using Moq;
using NUnit.Framework;
using Services.Services;
using Services.Tests.Helpers;
using Services.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using System.Threading.Tasks;

namespace Services.Tests
{

    public class ActivityAlgorithmTest
    {

        private readonly IActivityAlgorithmService activityAlgorithmService;
        private readonly ProjectGeneratorHelper projectGeneratorHelper;

        public ActivityAlgorithmTest()
        {
            Mock<IProjectService> projectServiceMock = new Mock<IProjectService>();
            Mock<IActivityAlgorithmRepository> activityRepoMock = new Mock<IActivityAlgorithmRepository>();
            activityRepoMock.Setup(m => m.GetActivityAlgorithmConfig()).Returns(Task.FromResult(new ProjectActivityConfig()
            {
                AverageLikeDateMultiplier = 1,
                ConnectedCollaboratorsMultiplier = 1,
                RecentCreatedDataMultiplier = 1,
                InstitutionMultiplier = 1,
                LikeDataMultiplier = 1,
                MetaDataMultiplier = 1,
                RepoScoreMultiplier = 1,
                UpdatedTimeMultiplier = 1
            }));
            projectServiceMock.Setup(m => m.UpdateActivityScore((new Mock<Project>()).Object));
            activityAlgorithmService = new ActivityAlgorithmService(activityRepoMock.Object, projectServiceMock.Object);
            projectGeneratorHelper = new ProjectGeneratorHelper();
        }

        [Test]
        public void ScoreProjectBasedOnLikes()
        {
            Project project = new Project();
            project.Likes =  new List<ProjectLike>()
                                                    {
                                                        projectGeneratorHelper.GetOldLike(),
                                                        projectGeneratorHelper.GetOldLike(),
                                                        projectGeneratorHelper.GetRecentLike()
                                                    };

            double score = activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
                                                                             {
                                                                                 new LikeDataPoint()
                                                                             });

            Assert.AreEqual((project.Likes.Count), score);

        }


        [Test]
        public void ScoreProjectBasedOnLikeDate()
        {
            Project project = new Project();
            project.Likes =  new List<ProjectLike>()
                             {
                                 projectGeneratorHelper.GetOldLike(),
                                 projectGeneratorHelper.GetRecentLike()
                             };

            double score = activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
                {
                    new AverageLikeDateDataPoint()
                });

            Assert.AreEqual(2, score);

        }



        [Test]
        public void ScoreProjectBasedOnConnectedCollaborators()
        {
            Project project = new Project();
            project.Collaborators =  new List<Collaborator>()
                             {
                                 new Collaborator(),
                                 new Collaborator(),
                                 new Collaborator(),
                                 new Collaborator(),
                                 new Collaborator(),
                             };

            double score = activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
                {
                    new ConnectedCollaboratorsDataPoint()
                });

            Assert.AreEqual(5, score);

        }


        [Test]
        public void OrderProjectsWithAlgorithm()
        {
            Project firstProject = projectGeneratorHelper.GetActiveProject();
            Project secondProject = projectGeneratorHelper.GetInactiveProject(10);
            Project thirdProject = projectGeneratorHelper.GetInactiveProject();
            Project fourthProject = projectGeneratorHelper.GetActiveProject(10);
            IEnumerable<Project> projects = new List<Project>()
                                     {
                                         firstProject,
                                         secondProject,
                                         thirdProject,
                                         fourthProject
                                     };

            List<Project> orderedProjects =  activityAlgorithmService.CalculateAllProjects(projects)
                                                                     .OrderByDescending(p => p.ActivityScore)
                                                                     .ToList();

            orderedProjects[0]
                .Should()
                .Be(fourthProject);
            orderedProjects[1]
                .Should()
                .Be(firstProject);
            orderedProjects[2]
                .Should()
                .Be(secondProject);
            orderedProjects[3]
                .Should()
                .Be(thirdProject);

        }

    }

}
