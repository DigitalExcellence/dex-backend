# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Added this very changelog - [#71](https://github.com/DigitalExcellence/dex-backend/issues/71)
- Setup basic unit test framework to ensure that the core functionality of the application works - [#65](https://github.com/DigitalExcellence/dex-backend/issues/65)
- Added example unittests for other contributors [#74](https://github.com/DigitalExcellence/dex-backend/issues/74)
- You can now authenticate swagger with the Identity server - [#101](https://github.com/DigitalExcellence/dex-backend/issues/101)
- Added issue & pull request templates (bug & report) - [#11](https://github.com/DigitalExcellence/dex-backend/issues/11)
- Changed errors to be compliant with RFC 7807, with guids for easy error searching - [#80](https://github.com/DigitalExcellence/dex-backend/issues/80)
- Added highlighted filter to search endpoint - [#57](https://github.com/DigitalExcellence/dex-backend/issues/57)
- Setup basic unit test framework to ensure that the core functionality of the application works - [#65](https://github.com/DigitalExcellence/dex-backend/issues/65)
- Added roles and authorization validation. - [#107](https://github.com/DigitalExcellence/dex-backend/issues/107)
- Added Wizard Controller and service - [#127](https://github.com/DigitalExcellence/dex-backend/issues/127)
- Added Wizard GitLab metadata - [#125](https://github.com/DigitalExcellence/dex-backend/issues/125)
- Added unittests for User service & repository - [#121](https://github.com/DigitalExcellence/dex-backend/issues/121)
- Added an endpoint to get information about the current user - [#141](https://github.com/DigitalExcellence/dex-backend/issues/141)
- Added pagination to the get all projects endpoint - [#156](https://github.com/DigitalExcellence/dex-backend/issues/156)
- Added sorting to the get all projects endpoint - [#157](https://github.com/DigitalExcellence/dex-backend/issues/157)
- Added filtering to the get all projects endpoint - [#161](https://github.com/DigitalExcellence/dex-backend/issues/161)
- Added error tracking and monitoring with Sentry - [#136](https://github.com/DigitalExcellence/dex-backend/issues/136)
- Support for fontys login - [#66](https://github.com/DigitalExcellence/dex-backend/issues/66)
- Added flag to indicate if email is public, show redacted email if not public - [#138](https://github.com/DigitalExcellence/dex-backend/issues/138)
- Added Integration tests using Postman, also tests different access control levels - [#40](https://github.com/DigitalExcellence/dex-backend/issues/40) 
- Added an endpoint to get highlights by a project identifier - [#174](https://github.com/DigitalExcellence/dex-backend/issues/174)
- Automated the deployment to our environments - [#60](https://github.com/DigitalExcellence/dex-backend/issues/60)
- Added docker compose to get the backend services running locally - [#179](https://github.com/DigitalExcellence/dex-backend/issues/179)
- Added a user self delete endpoint, allowing users to delete their own account - [#154](https://github.com/DigitalExcellence/dex-backend/issues/154)
- Added ability for users with PR role to now create en see embeds for all projects - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Added unit testing for the GitLab source - [#181](https://github.com/DigitalExcellence/dex-backend/issues/181)
- Added unit testing for the GitHub source - [#182](https://github.com/DigitalExcellence/dex-backend/issues/182)
- Added unit testing for the source manager - [#173](https://github.com/DigitalExcellence/dex-backend/issues/173)

### Changed

- Return Unauthorized instead of Bad Request when not allowed to perform action in controller - [#132](https://github.com/DigitalExcellence/dex-backend/issues/132)
- Changed the migrations and seeding of the data - [#134](https://github.com/DigitalExcellence/dex-backend/issues/134)
- Get user from the session & add current user to project. - [#139](https://github.com/DigitalExcellence/dex-backend/issues/139)
- Changed Student reference to be named identity. - [#145](https://github.com/DigitalExcellence/dex-backend/issues/145)
- Changed the login flow to the identity to support direct access to external providers. - [#165](https://github.com/DigitalExcellence/dex-backend/issues/165)
- Changed endpoint to return all roles from /api/role/roles to /api/role - [#168](https://github.com/DigitalExcellence/dex-backend/issues/168)
- Improved logging on exceptions and removed some possible null reference exception flows - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Changed how much information about a user is being returned when requesting project for instance - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)

### Deprecated

### Removed

- Removed user from search result resource - [#129](https://github.com/DigitalExcellence/dex-backend/issues/129)
- Removed linked service resource inside the user resource, it was not being used - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Removed Internet Information Services webserver from the launchsettings, we only want to use Kestrel - [#105](https://github.com/DigitalExcellence/dex-backend/issues/105)

### Fixed

- Fixed Get Highlight endpoint using wrong parameter, project id instead of highlight id - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed newly created users not having any role, leading to authorization issues - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed Get Highlight not returning a project - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed issue where retrieving highlights would not redact user information - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed issue where backend applications like Postman were unable to make requests due to UserExtension not checking for client_role - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed issue where incorrect guid validation would lead to internal server errors - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed issue where users were able to delete some roles that were critical for the platform - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed role update endpoint returning internal server error - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Fixed issue where the user was not returned on the /api/Project endpoint (which returns all the projects) - [#169](https://github.com/DigitalExcellence/dex-backend/issues/169)


### Security
