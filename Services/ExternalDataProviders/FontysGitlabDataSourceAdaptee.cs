using AutoMapper;
using Microsoft.Extensions.Configuration;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ExternalDataProviders
{
    public class FontysGitlabDataSourceAdaptee : GitlabDataSourceAdaptee
    {
        public FontysGitlabDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper, IConfiguration configuration) : base(restClientFactory, mapper, configuration)
        {
            Title = "FontysGitlab";
            Guid = "6a73f5ee-e7a5-4ef5-b874-0da0993d6f13";
        }
    }
}
