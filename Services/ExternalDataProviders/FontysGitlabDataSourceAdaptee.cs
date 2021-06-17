using AutoMapper;
using Microsoft.Extensions.Configuration;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.ExternalDataProviders.Resources;

namespace Services.ExternalDataProviders
{
    public class FontysGitlabDataSourceAdaptee : GitlabDataSourceAdaptee
    {
        //We have to do it like this, since the constructor of the GitlabDataSourceAdaptee gets called before our constructor, causing an exception
        public override string Title { get => "FontysGitlab"; set => base.Title = value; }
        public FontysGitlabDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper, IConfiguration configuration) : base(configuration, restClientFactory, mapper)
        {
            Guid = "6a73f5ee-e7a5-4ef5-b874-0da0993d6f13";
            BaseApiUrl = "https://git.fhict.nl/api/v4/";
            BaseUrl = "https://git.fhict.nl/";
            AlwaysRequiresAuthentication = true;
            IsVisible = false;
        }

        /// <summary>
        /// Stop fetching readme for now as we cannot authorize towards protected uris yet.
        /// </summary>
        /// <param name="readmeUri"></param>
        /// <returns></returns>
        public override Task<string> FetchReadme(string readmeUri)
        {
            return Task.FromResult<string>(null);
        }
    }
}
