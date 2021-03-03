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
    ///     Viewmodel for the project gitlab data source.
    /// </summary>
    public class GitlabDataSourceResourceResult
    {

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the readme URL.
        /// </summary>
        /// <value>
        ///     The readme URL.
        /// </value>
        [JsonProperty("readme_url")]
        public string ReadmeUrl { get; set; }

        /// <summary>
        ///     Gets or sets the web URL.
        /// </summary>
        /// <value>
        ///     The web URL.
        /// </value>
        [JsonProperty("web_url")]
        public string WebUrl { get; set; }

    }

    /// <summary>
    ///     Viewmodel for the gitlab data source user.
    /// </summary>
    public class GitlabDataSourceUserResourceResult
    {

        /// <summary>
        ///     Gets or sets a value for the Id property.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Username property.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Email property.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Name property.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value for the State property.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets a value for the AvatarUrl property.
        /// </summary>
        [JsonProperty("avatar_url")]
        public Uri AvatarUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value for the WebUrl property.
        /// </summary>
        [JsonProperty("web_url")]
        public Uri WebUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value for the CreatedAt property.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Bio property.
        /// </summary>
        [JsonProperty("bio")]
        public string Bio { get; set; }

        /// <summary>
        ///     Gets or sets a value for the BioHtml property.
        /// </summary>
        [JsonProperty("bio_html")]
        public string BioHtml { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Location property.
        /// </summary>
        [JsonProperty("location")]
        public object Location { get; set; }

        /// <summary>
        ///     Gets or sets a value for the PublicEmail property.
        /// </summary>
        [JsonProperty("public_email")]
        public string PublicEmail { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Skype property.
        /// </summary>
        [JsonProperty("skype")]
        public string Skype { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Linkedin property.
        /// </summary>
        [JsonProperty("linkedin")]
        public string Linkedin { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Twitter property.
        /// </summary>
        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        /// <summary>
        ///     Gets or sets a value for the WebsiteUrl property.
        /// </summary>
        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Organization property.
        /// </summary>
        [JsonProperty("organization")]
        public string Organization { get; set; }

        /// <summary>
        ///     Gets or sets a value for the LastSignInAt property.
        /// </summary>
        [JsonProperty("last_sign_in_at")]
        public DateTimeOffset LastSignInAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ConfirmAt property.
        /// </summary>
        [JsonProperty("confirmed_at")]
        public DateTimeOffset ConfirmedAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ThemeId property.
        /// </summary>
        [JsonProperty("theme_id")]
        public long ThemeId { get; set; }

        /// <summary>
        ///     Gets or sets a value for the LastActivityOn property.
        /// </summary>
        [JsonProperty("last_activity_on")]
        public DateTimeOffset LastActivityOn { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ColorSchemeId property.
        /// </summary>
        [JsonProperty("color_scheme_id")]
        public long ColorSchemeId { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ProjectsLimit property.
        /// </summary>
        [JsonProperty("projects_limit")]
        public long ProjectsLimit { get; set; }

        /// <summary>
        ///     Gets or sets a value for the CurrentSignInAt property.
        /// </summary>
        [JsonProperty("current_sign_in_at")]
        public DateTimeOffset CurrentSignInAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Identities property.
        /// </summary>
        [JsonProperty("identities")]
        public GitlabDataSourceIdentityResourceResult[] Identities { get; set; }

        /// <summary>
        ///     Gets or sets a value for the CanCreateGroup property.
        /// </summary>
        [JsonProperty("can_create_group")]
        public bool CanCreateGroup { get; set; }

        /// <summary>
        ///     Gets or sets a value for the CanCreateProject property.
        /// </summary>
        [JsonProperty("can_create_project")]
        public bool CanCreateProject { get; set; }

        /// <summary>
        ///     Gets or sets a value for the TwoFactorEnabled property.
        /// </summary>
        [JsonProperty("two_factor_enabled")]
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value for the External property.
        /// </summary>
        [JsonProperty("external")]
        public bool External { get; set; }

        /// <summary>
        ///     Gets or sets a value for the PrivateProfile property.
        /// </summary>
        [JsonProperty("private_profile")]
        public bool PrivateProfile { get; set; }

    }

    /// <summary>
    ///     Viewmodel for the gitlab data source identity.
    /// </summary>
    public class GitlabDataSourceIdentityResourceResult
    {

        /// <summary>
        ///     Gets or sets a value for the Provider property.
        /// </summary>
        [JsonProperty("provider")]
        public string Provider { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ExternUid property.
        /// </summary>
        [JsonProperty("extern_uid")]
        public string ExternUid { get; set; }

    }

}
