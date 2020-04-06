using GitLabApiClient;
using GitLabApiClient.Models.Projects.Requests;
using GitLabApiClient.Models.Projects.Responses;
using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Sources
{
    public class GitLabSource : ISource
    {
        private GitLabClient client;

        public GitLabSource()
        {
            this.client = new GitLabClient("https://git.fhict.nl", "q6C7ygyGyQ5RjvACxWiy");
        }

        public void getSource(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SearchResult>> Search(string searchTerm)
        {
            //FOR NOW THIS RETURNS ALL OF THE INTERNAL PROJECTS, FILTERING NEEDS TO BE DONE BASED ON SEARCHQUERYPARAMETERS WHICH WILL BE RECEIVED VIA
            //PARAMETERS INSTEAD OF THE SEARTERM-STRING.
            List<SearchResult> searchResults = new List<SearchResult>();
            var projects = await this.client.Projects.GetAsync(project => project.Visibility = QueryProjectVisibilityLevel.Internal);
            foreach(Project project in projects) {
                searchResults.Add(new SearchResult()
                {
                    Name = project.Name,
                    Description = project.Description,
                    Type = project.Visibility.ToString(),
                    Uri = project.HttpUrlToRepo
                });
            }
            return searchResults;
        }
    }
}