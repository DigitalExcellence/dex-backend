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

### Changed

- Return Unauthorized instead of Bad Request when not allowed to perform action in controller - [#132](https://github.com/DigitalExcellence/dex-backend/issues/132)
- Changed the migrations and seeding of the data - [#134](https://github.com/DigitalExcellence/dex-backend/issues/134)
- Get user from the session & add current user to project. - [#139](https://github.com/DigitalExcellence/dex-backend/issues/139)
- Changed Student reference to be named identity. - [#145](https://github.com/DigitalExcellence/dex-backend/issues/145)

### Deprecated

### Removed

- Removed user from search result resource - [#129](https://github.com/DigitalExcellence/dex-backend/issues/129)

### Fixed

### Security
