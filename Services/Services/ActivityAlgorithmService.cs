using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Services.Services
{
    public interface IActivityAlgorithmService
    {
        List<Project> CalculateAllProjects();
        double CalculateProjectActivityScore(Project project);
        bool SetProjectActivityScore(Project project, double score);

    }
    public class ActivityAlgorithmService : IActivityAlgorithmService
    {
        public ActivityAlgorithmService()
        {

        }

        public List<Project> CalculateAllProjects()
        {
            throw new NotImplementedException();
        }

        public double CalculateProjectActivityScore(Project project)
        {
            throw new NotImplementedException();
        }

        public bool SetProjectActivityScore(Project project, double score)
        {
            throw new NotImplementedException();
        }
    }
}
