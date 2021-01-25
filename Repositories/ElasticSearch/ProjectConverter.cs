using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ElasticSearch
{
    public static class ProjectConverter
    {
        public static List<ESProjectFormatDTO> ProjectsToProjectESDTOs(List<Project> projectsToConvert)
        {
            
            List<ESProjectFormatDTO> convertedProjects = new List<ESProjectFormatDTO>();
            foreach(Project project in projectsToConvert)
            {
                ESProjectFormatDTO convertedProject = new ESProjectFormatDTO();
                ProjectToESProjectDTO(project, convertedProject);
                convertedProjects.Add(convertedProject);

            }
            return convertedProjects;
        }

        public static void ProjectToESProjectDTO(Project project, ESProjectFormatDTO convertedProject)
        {
            List<int> likes = new List<int>();
            if (project.Likes != null)
            {
                foreach(ProjectLike projectLike in project.Likes)
                {
                    likes.Add(projectLike.UserId);
                }
            }            
            convertedProject.Description = project.Description;
            convertedProject.ProjectName = project.Name;
            convertedProject.Id = project.Id;
            convertedProject.Created = project.Created;
            convertedProject.Likes = likes;
        }
    }
}
