using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectTag> ProjectTags { get; set; }
    }

    public class ProjectTag
    {
        public ProjectTag(Tag tag, Project project)
        {
            this.Tag = tag;
            this.Project = project;
        }

        public ProjectTag(){}

        public int Id { get; set; }
        public Tag Tag { get; set; }
        public Project Project { get; set; }
    }
}
