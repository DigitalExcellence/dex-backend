using Models;
using NUnit.Framework;
using Services.Services;
using Services.Tests.Helpers;
using Services.Tests.Base;
using System;
using System.Collections.Generic;

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

    }

}
