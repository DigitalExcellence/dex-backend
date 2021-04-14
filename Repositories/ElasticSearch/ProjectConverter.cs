using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ElasticSearch
{
    public static class ProjectConverter
    {
        public static List<ESProjectDTO> ProjectsToProjectESDTOs(List<Project> projectsToConvert)
        {
            
            List<ESProjectDTO> convertedProjects = new List<ESProjectDTO>();
            foreach(Project project in projectsToConvert)
            {
                ESProjectDTO convertedProject = ProjectToESProjectDTO(project);
                convertedProjects.Add(convertedProject);

            }
            return convertedProjects;
        }

        public static ESProjectDTO ProjectToESProjectDTO(Project project)
        {
            ESProjectDTO convertedProject = new ESProjectDTO();
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
            return convertedProject;
        }
    }
}
