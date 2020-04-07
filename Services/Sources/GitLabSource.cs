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
            ProjectQueryOptions queryOptions = null;
            foreach(SearchQueryParameter queryParameter in queryParameters)
            {
                switch(queryParameter.Type)
                {
                    case SearchQueryParameterType.NAME:
                        queryOptions.Filter = queryParameter.Value.ToString();
                        break;
                    case SearchQueryParameterType.VISIBILITY:
                        if (queryParameter.Value.Equals("PRIVATE"))
                            queryOptions.Visibility = QueryProjectVisibilityLevel.Private;
                        if (queryParameter.Value.Equals("INTERNAL"))
                            queryOptions.Visibility = QueryProjectVisibilityLevel.Internal;
                        if (queryParameter.Value.Equals("PUBLIC"))
                            queryOptions.Visibility = QueryProjectVisibilityLevel.Public;
                        if (queryParameter.Value.Equals("ALL"))
                            queryOptions.Visibility = QueryProjectVisibilityLevel.All;
                        break;
                }
            }

            List<SearchResult> searchResults = new List<SearchResult>();
            if(queryParameters != null)
            {
                var projects = await this.client.Projects.GetAsync(project => project.Visibility = QueryProjectVisibilityLevel.Internal);
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