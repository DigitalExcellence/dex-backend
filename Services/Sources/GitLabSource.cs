using GitLabApiClient;
using GitLabApiClient.Models.Projects.Requests;
using GitLabApiClient.Models.Projects.Responses;
using Search;
using Sources;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            //Generate the object which will be used for filtering in the gitlab api.
            //Reflection is needed here to create an instance of the internal ProjectQueryOptions, in order to dynamically generate the filter query object.
            Type type = typeof(ProjectQueryOptions);
            ProjectQueryOptions queryOptions = (ProjectQueryOptions) type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, null, null, null);
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
            }
            //Do the API call and return the searchresults.
            List<SearchResult> searchResults = new List<SearchResult>();
            if(queryParameters != null)
            {
                IList<Project> projects = await this.client.Projects.GetAsync(project => { 
                    project.Owned = queryOptions.Owned;
                    project.IncludeStatistics = queryOptions.IncludeStatistics;
                    project.Filter = queryOptions.Filter;
                    project.Archived = queryOptions.Archived;
                    project.IsMemberOf = queryOptions.IsMemberOf;
                    project.Order = queryOptions.Order;
                    project.Owned = queryOptions.Owned;
                    project.Simple = queryOptions.Simple;
                    project.SortOrder = queryOptions.SortOrder;
                    project.Starred = queryOptions.Starred;
                    project.UserId = queryOptions.UserId;
                    project.Visibility = queryOptions.Visibility;
                    project.WithIssuesEnabled = queryOptions.WithIssuesEnabled;
                    project.WithMergeRequestsEnabled = queryOptions.WithMergeRequestsEnabled;
                });

                //The code below is an alternative to reflection but is way less dynamic, and more code thus less readable.
                /*
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

                */
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