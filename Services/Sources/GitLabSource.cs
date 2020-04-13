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

        public async Task<IEnumerable<SearchResult>> Search(List<SearchQueryParameter> queryParameters)
        {
            //TODO: Replace searchfunctionality by somewhat similar code as this below:
            /*
            ProjectQueryOptions queryOptions = new ProjectQueryOptions();
            foreach(SearchQueryParameter queryParameter in queryParameters)
            {
                if (queryParameter.Type == SearchQueryParameterType.NAME)
                {
                    queryOptions.Filter = queryParameter.Value.ToString();
                }
                else if (queryParameter.Type == SearchQueryParameterType.VISIBILITY)
                {
                    if (queryParameter.Value.Equals("PRIVATE"))
                        queryOptions.Visibility = QueryProjectVisibilityLevel.Private;
                    else if (queryParameter.Value.Equals("INTERNAL"))
                        queryOptions.Visibility = QueryProjectVisibilityLevel.Internal;
                    else if (queryParameter.Value.Equals("PUBLIC"))
                        queryOptions.Visibility = QueryProjectVisibilityLevel.Public;
                    else if (queryParameter.Value.Equals("ALL"))
                        queryOptions.Visibility = QueryProjectVisibilityLevel.All;
                }
                   
            }*/

            List<SearchResult> searchResults = new List<SearchResult>();
            if(queryParameters != null)
            {
                IList<Project> projects = new List<Project>();
                //CONTAINS BOTH NAME AND VISIBILITY PARAMETERS
                if (queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME) != null && queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY) != null)
                {
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "PRIVATE")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Private; project.Filter = queryParameters[queryParameters.IndexOf(queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME))].Value; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "INTERNAL")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Internal; project.Filter = queryParameters[queryParameters.IndexOf(queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME))].Value; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "PUBLIC")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Public; project.Filter = queryParameters[queryParameters.IndexOf(queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME))].Value; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "ALL")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.All; project.Filter = queryParameters[queryParameters.IndexOf(queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME))].Value; });
                }
                //CONTAINS ONLY NAME PARAMETER
                if (queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME) != null && queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY) == null)
                {
                    projects = await this.client.Projects.GetAsync(project => { project.Filter = queryParameters[queryParameters.IndexOf(queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME))].Value; });
                }
                //CONTAINS ONLY TYPE PARAMETER
                if (queryParameters.Find(p => p.Type == SearchQueryParameterType.NAME) == null && queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY) != null)
                {
                    if(queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "PRIVATE")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Private; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "INTERNAL")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Internal; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "PUBLIC")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.Public; });
                    if (queryParameters.Find(p => p.Type == SearchQueryParameterType.VISIBILITY).Value == "ALL")
                        projects = await this.client.Projects.GetAsync(project => { project.Visibility = QueryProjectVisibilityLevel.All; });
                }
                foreach (Project project in projects)
                {
                    searchResults.Add(new SearchResult()
                    {
                        Name = project.Name,
                        Description = project.Description,
                        Type = project.Visibility.ToString(),
                        Uri = project.HttpUrlToRepo
                    });
                }
            }
            return searchResults;
        }
    }
}