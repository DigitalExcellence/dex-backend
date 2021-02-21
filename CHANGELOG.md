# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]



### Added

- Added wizard to automatically import project with a public and an oauth flow - [326](https://github.com/DigitalExcellence/dex-backend/issues/326)

### Changed


### Deprecated


### Removed


### Fixed


### Security


## Release v.1.0.1-beta - 31-01-2021



### Added

- Added ability to make projects private to a specific institution - [#310](https://github.com/DigitalExcellence/dex-backend/issues/310)

### Changed

- Refactored Message Broker Publisher to make use of app settings and connection factory - [#356](https://github.com/DigitalExcellence/dex-backend/issues/356)
- Project Likes now returns ProjectLiker instead of CreatorOfProject attribute - [#341](https://github.com/DigitalExcellence/dex-backend/issues/341)

### Fixed

- Refactored Postman tests - [#328](https://github.com/DigitalExcellence/dex-backend/issues/328)
- Resolve update institution id for data officer bug - [#352](https://github.com/DigitalExcellence/dex-backend/issues/352)

## Release v.0.9.0-beta - 09-12-2020



### Added

- Automatically link users to their institution - [#295](https://github.com/DigitalExcellence/dex-backend/issues/295)
- Added call to actions for projects and call to action options - [#312](https://github.com/DigitalExcellence/dex-backend/issues/312)
- Collaborators are now included on the project overview page - [#317](https://github.com/DigitalExcellence/dex-backend/issues/317)
- Added new endpoint for ability to like and unlike projects - [#229](https://github.com/DigitalExcellence/dex-backend/issues/229)
- Project retrieval endpoints now include likes - [#329](https://github.com/DigitalExcellence/dex-backend/issues/329)

### Fixed

- Fixed issue where unused project icons where left in the database & File System - [#271](https://github.com/DigitalExcellence/dex-backend/issues/271)
- Refactored Postman CLI files to make them work from Postman folder - [#304](https://github.com/DigitalExcellence/dex-backend/issues/304)
- Fixed issue where searching for a project did not include the project icon - [#307](https://github.com/DigitalExcellence/dex-backend/issues/307)
- Fixed issue where project icons would get deleted when they should not - [#332](https://github.com/DigitalExcellence/dex-backend/issues/332)
- Fixes issue where retrieving projects performed badly due to large amount of collaborators - [#331](https://github.com/DigitalExcellence/dex-backend/issues/331)


## Release v.0.8.0-beta - 06-11-2020



### Added

- Added a fileuploader which gives the opportunity to upload files and icons - [#217](https://github.com/DigitalExcellence/dex-backend/issues/217)
- Add Postman tests to pipeline - [#189](https://github.com/DigitalExcellence/dex-backend/issues/189)
- Added function to follow users and let users follow projects - [#228](https://github.com/DigitalExcellence/dex-backend/pull/258)
- Added a new dex user that can be used to add projects manually - [#270](https://github.com/DigitalExcellence/dex-backend/issues/270)
- Added data officer role and CRUD functionalities for institutions - [#265](https://github.com/DigitalExcellence/dex-backend/issues/265)
- Added notification system - [#256](https://github.com/DigitalExcellence/dex-backend/issues/256)

### Fixed

- Fixed issue where swagger authorization was not working when running in docker-compose - [#200](https://github.com/DigitalExcellence/dex-backend/issues/200)
- Fixed issue with search functionality being too extensive. Now matching whole strings only - [#202](https://github.com/DigitalExcellence/dex-backend/issues/202)
- Fixed issue where highlights were not returning the end date & start date - [#296](https://github.com/DigitalExcellence/dex-backend/issues/296)



## Release v.0.7.0-beta - 09-10-2020

### Added
- Added descriptions for Project Highlights - [#219](https://github.com/DigitalExcellence/dex-backend/issues/219)

### Changed

- Changed DeX Frontend client to use code flow instead of implicit flow to fix silent refresh - [#246](https://github.com/DigitalExcellence/dex-backend/issues/246)
- Improved swagger documentation - [#225](https://github.com/DigitalExcellence/dex-backend/issues/225)

### Fixed

- Fixed a bug where GetAllHighlights endpoint returned status code 404 when empty. - [#207](https://github.com/DigitalExcellence/dex-backend/issues/207)
- Fixed issue where local docker-compose would not work due to missing connection string - [#234](https://github.com/DigitalExcellence/dex-backend/issues/234)


## Release v.0.6.1-beta - 16-09-2020

### Changed

- Changed all file line endings from CRLF to LF and added the .gitattributes to enforce it - [#163](https://github.com/DigitalExcellence/dex-backend/issues/163)

### Fixed

- Fixed issue resulting in people being unable to sign up with a new account - [#231](https://github.com/DigitalExcellence/dex-backend/issues/231)
- Fixed issue where highlights were sending too much information. - [#205](https://github.com/DigitalExcellence/dex-backend/issues/205)

## Release v.0.6-beta - 22-06-2020

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
- Added unit testing for the role service & repository - [#172](https://github.com/DigitalExcellence/dex-backend/issues/172)
- Added unit testing for the Search service - [#183](https://github.com/DigitalExcellence/dex-backend/issues/183)
- Added sanitization for project Description - [#198](https://github.com/DigitalExcellence/dex-backend/issues/198)
- Added redirection to the frontend after logout from the identity server - [#216](https://github.com/DigitalExcellence/dex-backend/issues/216)

### Changed

- Return Unauthorized instead of Bad Request when not allowed to perform action in controller - [#132](https://github.com/DigitalExcellence/dex-backend/issues/132)
- Changed the migrations and seeding of the data - [#134](https://github.com/DigitalExcellence/dex-backend/issues/134)
- Get user from the session & add current user to project. - [#139](https://github.com/DigitalExcellence/dex-backend/issues/139)
- Changed Student reference to be named identity. - [#145](https://github.com/DigitalExcellence/dex-backend/issues/145)
- Changed the login flow to the identity to support direct access to external providers. - [#165](https://github.com/DigitalExcellence/dex-backend/issues/165)
- Changed endpoint to return all roles from /api/role/roles to /api/role - [#168](https://github.com/DigitalExcellence/dex-backend/issues/168)
- Improved logging on exceptions and removed some possible null reference exception flows - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Changed how much information about a user is being returned when requesting project for instance - [#178](https://github.com/DigitalExcellence/dex-backend/issues/178)
- Changed in memory user store to a persistent memory store for the identity server - [#159](https://github.com/DigitalExcellence/dex-backend/issues/159)

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
- Fixed issue where the projects endpoint returned unexpected results when using filters - [#185](https://github.com/DigitalExcellence/dex-backend/issues/185)
- Fixed issue where most fields in the open-id configuration of our environments used http instead of https - [#210](https://github.com/DigitalExcellence/dex-backend/issues/210)
- Fixed issue where the project update endpoint would update the project user - [#213](https://github.com/DigitalExcellence/dex-backend/issues/213)
