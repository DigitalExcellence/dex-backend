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

namespace Services.Tests
{

    public class ActivityAlgorithmTest
    {

        private IActivityAlgorithmService _activityAlgorithmService;
        private ProjectGeneratorHelper _projectGeneratorHelper;

        public ActivityAlgorithmTest()
        {
            _activityAlgorithmService = new ActivityAlgorithmService();
            _projectGeneratorHelper = new ProjectGeneratorHelper();
        }

        [Test]
        public void ScoreProjectBasedOnLikes()
        {
            Project project = new Project();
            project.Likes =  new List<ProjectLike>()
                                                    {
                                                        _projectGeneratorHelper.GetOldLike(),
                                                        _projectGeneratorHelper.GetOldLike(),
                                                        _projectGeneratorHelper.GetRecentLike()
                                                    };

            double score = _activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
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
                                 _projectGeneratorHelper.GetOldLike(),
                                 _projectGeneratorHelper.GetOldLike(),
                                 _projectGeneratorHelper.GetRecentLike()
                             };

            double score = _activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
                {
                    new AverageLikeDateDataPoint()
                });

            Assert.AreEqual(1, score);

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

            double score = _activityAlgorithmService.CalculateProjectActivityScore(project, new List<AbstractDataPoint>
                {
                    new ConnectedCollaboratorsDataPoint()
                });

            Assert.AreEqual(5, score);

        }


        [Test]
        public void OrderProjectsWithAlgorithm()
        {
            Project firstProject = _projectGeneratorHelper.GetActiveProject();
            Project secondProject = _projectGeneratorHelper.GetInactiveProject(10);
            Project thirdProject = _projectGeneratorHelper.GetInactiveProject();
            Project fourthProject = _projectGeneratorHelper.GetActiveProject(10);
            IEnumerable<Project> projects = new List<Project>()
                                     {
                                         firstProject,
                                         secondProject,
                                         thirdProject,
                                         fourthProject
                                     };

            List<Project> orderedProjects = _activityAlgorithmService.CalculateAllProjects(projects)
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
