/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Newtonsoft.Json;
using System;

namespace Services.ExternalDataProviders.Resources
{
    /// <summary>
    /// Viewmodel for the project github data source.
    /// </summary>
    public class GithubDataSourceResourceResult
    {
        /// <summary>
        /// Gets or sets a value for the Id property.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value for the NodeId property.
        /// </summary>
        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets a value for the Name property.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for the FullName property.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value for the Description property.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value for the IsPrivate property.
        /// </summary>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets a value for the Owner property.
        /// </summary>
        [JsonProperty("owner")]
        public GithubDataSourceOwnerResourceResult Owner { get; set; }

        /// <summary>
        /// Gets or sets a value for the Size property.
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets a value for the StargazersCount property.
        /// </summary>
        [JsonProperty("stargazers_count")]
        public long StargazersCount { get; set; }

        /// <summary>
        /// Gets or sets a value for the WatchersCount property.
        /// </summary>
        [JsonProperty("watchers_count")]
        public long WatchersCount { get; set; }

        /// <summary>
        /// Gets or sets a value for the Language property.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets a value for the HasIssues property.
        /// </summary>
        [JsonProperty("has_issues")]
        public bool HasIssues { get; set; }

        /// <summary>
        /// Gets or sets a value for the HasProjects property.
        /// </summary>
        [JsonProperty("has_projects")]
        public bool HasProjects { get; set; }

        /// <summary>
        /// Gets or sets a value for the HasDownloads property.
        /// </summary>
        [JsonProperty("has_downloads")]
        public bool HasDownloads { get; set; }

        /// <summary>
        /// Gets or sets a value for the HasWiki property.
        /// </summary>
        [JsonProperty("has_wiki")]
        public bool HasWiki { get; set; }

        /// <summary>
        /// Gets or sets a value for the HasPages property.
        /// </summary>
        [JsonProperty("has_pages")]
        public bool HasPages { get; set; }

        /// <summary>
        /// Gets or sets a value for the ForksCount property.
        /// </summary>
        [JsonProperty("forks_count")]
        public long ForksCount { get; set; }

        /// <summary>
        /// Gets or sets a value for the MirrorUrl property.
        /// </summary>
        [JsonProperty("mirror_url")]
        public object MirrorUrl { get; set; }

        /// <summary>
        /// Gets or sets a value for the Archived property.
        /// </summary>
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        /// <summary>
        /// Gets or sets a value for the Disabled property.
        /// </summary>
        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value for the License property.
        /// </summary>
        [JsonProperty("license")]
        public GithubDataSourceLicenseResourceResult License { get; set; }

        /// <summary>
        /// Gets or sets a value for the Forks property.
        /// </summary>
        [JsonProperty("forks")]
        public long Forks { get; set; }

        /// <summary>
        /// Gets or sets a value for the OpenIssues property.
        /// </summary>
        [JsonProperty("open_issues")]
        public long OpenIssues { get; set; }

        /// <summary>
        /// Gets or sets a value for the Watchers property.
        /// </summary>
        [JsonProperty("watchers")]
        public long Watchers { get; set; }

        /// <summary>
        /// Gets or sets a value for the DefaultBranch property.
        /// </summary>
        [JsonProperty("default_branch")]
        public string DefaultBranch { get; set; }

        /// <summary>
        /// Gets or sets a value for the TempCloneToken property.
        /// </summary>
        [JsonProperty("temp_clone_token")]
        public object TempCloneToken { get; set; }

        /// <summary>
        /// Gets or sets a value for the NetworkCount property.
        /// </summary>
        [JsonProperty("network_count")]
        public long NetworkCount { get; set; }

        /// <summary>
        /// Gets or sets a value for the SubscribersCount property.
        /// </summary>
        [JsonProperty("subscribers_count")]
        public long SubscribersCount { get; set; }

    }

    /// <summary>
    /// Viewmodel for the github data source owner source.
    /// </summary>
    public class GithubDataSourceOwnerResourceResult
    {

        /// <summary>
        /// Gets or sets a value for the Login property.
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets a value for the Id property.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a value for the NodeId property.
        /// </summary>
        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets a value for the AvatarUrl property.
        /// </summary>
        [JsonProperty("avatar_url")]
        public Uri AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets a value for the GravatarId property.
        /// </summary>
        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        /// <summary>
        /// Gets or sets a value for the Url property.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets a value for the HtmlUrl property.
        /// </summary>
        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        /// <summary>
        /// Gets or sets a value for the Type property.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value for the SiteAdmin property.
        /// </summary>
        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }

    }

    /// <summary>
    /// Viewmodel for the github data source license source.
    /// </summary>
    public class GithubDataSourceLicenseResourceResult
    {

        /// <summary>
        /// Gets or sets a value for the Key property.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value for the Name property.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for the SpdxId property.
        /// </summary>
        [JsonProperty("spdx_id")]
        public string SpdxId { get; set; }

        /// <summary>
        /// Gets or sets a value for the Url property.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets a value for the NodeId property.
        /// </summary>
        [JsonProperty("node_id")]
        public string NodeId { get; set; }
    }
}
