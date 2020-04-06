using GitLabApiClient;
using System;

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

        public void Search(string searchTerm)
        {

        }
    }
}