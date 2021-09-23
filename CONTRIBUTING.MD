# Contribution Guidelines

First off, thank you for taking the time to contribute!🎉💪

The following is a set of guidelines for contributing. These are mostly guidelines, not rules.
Use your best judgment, and feel free to propose changes to this document in a pull request.

## Table of contents

- [Contribution Guidelines](#contribution-guidelines)
  - [Table of contents](#table-of-contents)
  - [Why?](#why)
  - [Before Getting Started](#before-getting-started)
    - [Learn about our code of conduct](#learn-about-our-code-of-conduct)
    - [Learn about our contribution models](#learn-about-our-contribution-models)
    - [Got a Question or Problem?](#got-a-question-or-problem)
  - [Different contributions](#different-contributions)
    - [Found a bug?](#found-a-bug)
    - [Found a security vulnerability?](#found-a-security-vulnerability)
    - [Missing a Feature?](#missing-a-feature)
    - [Want to improve the documentation?](#want-to-improve-the-documentation)
  - [Submission Guidelines](#submission-guidelines)
    - [Submitting an Issue](#submitting-an-issue)
    - [Submitting a Pull Request (PR)](#submitting-a-pull-request-pr)
      - [Github Model](#github-model)
    - [Reviewing a Pull Request](#reviewing-a-pull-request)
  - [Your First Contribution](#your-first-contribution)
  - [Coding Guidelines](#coding-guidelines)

## Why?

Following these guidelines helps to communicate that you respect the time of the developers managing and developing this open-source project. In return, they should reciprocate that respect in addressing your issue, assessing changes, and helping you finalize your pull requests.

## Before Getting Started

### Learn about our code of conduct

Be respectful.

### Got a Question or Problem?

If you have questions about this project please ask the question by submitting an issue
in this repository.

## Different contributions

There are many ways to contribute, from writing tutorials, improving the documentation, submitting bug reports and feature requests, or writing code that can be incorporated into the project itself.

### Found a bug?

If you find a bug in the source code or a mistake in the documentation, you can help us by [submitting an issue](#submitting-an-issue) to our Github Repository.

### Found a security vulnerability?

If you discover a vulnerability in our software, please contact a code owner directly and report it appropriately.
Do not submit an issue, unless asked to.

### Missing a Feature?

You can request a new feature by [submitting an issue](#submitting-an-issue) to our GitHub Repository. If you would like to implement a new feature, please consider the size of the change in order to determine the right steps to proceed:

For a Major Feature, first, open an issue and outline your proposal so that it can be discussed. This process allows us to better coordinate our efforts, prevent duplication of work, and help you to craft the change so that it is successfully accepted into the project.

Note: Adding a new topic to the documentation, or significantly re-writing a topic, counts as a major feature.

Small Features can be crafted and directly submitted as a Pull Request.

### Want to improve the documentation?

If you want to help improve the docs, it's a good idea to let others know what you're working on to minimize duplication of effort.
Create a new issue (or comment on a related existing one) to let others know what you're working on.

## Submission Guidelines

### Submitting an Issue

Before you submit an issue, please search the issue tracker, maybe an issue for your problem already exists and the discussion might inform you of workarounds readily available.

We want to fix all the issues as soon as possible, but before fixing a bug we need to reproduce and confirm it. In order to reproduce bugs, we require that you provide minimal reproduction. Having a minimal reproducible scenario gives us a wealth of important information without going back and forth with you with additional questions.

A minimal reproduction allows us to quickly confirm a bug (or point out a coding problem) as well as confirm that we are fixing the right problem.

We require minimal reproduction to save maintainers time and ultimately be able to fix more bugs. Often, developers find coding problems themselves while preparing a minimal reproduction. We understand that sometimes it might be hard to extract essential bits of code from a larger codebase but we really need to isolate the problem before we can fix it.

Unfortunately, we are not able to investigate/fix bugs without minimal reproduction, so if we don't hear back from you, we are going to close an issue that doesn't have enough info to be reproduced.

You can file new issues by selecting from our new issue templates and filling out the issue template.

### Submitting a Pull Request (PR)

Before you submit your Pull Request (PR) consider the following guidelines:

1. Search the Github Repository for an open or closed PR that relates to your submission.
   You don't want to duplicate existing efforts.

1. Be sure that an issue describes the problem you're fixing, or documents the design for the feature you'd like to add.
   Discussing the design upfront helps to ensure that we're ready to accept your work.

1. Clone the repository.

1. Make your changes in a new git branch:

     ```shell
     git checkout -b my-fix-branch master
     ```

1. Create your patch, include tests if necessary.
  a. Ensure that all tests pass.

1. Commit your changes using a descriptive commit message ([follow commit guidelines](#commit-guidelines)).

     ```shell
     git commit --all
     ```

    Note: the optional commit `-a` command-line option will automatically "add" and "rm" edited files.

1. Push your branch to GitHub:

    ```shell
    git push origin my-fix-branch
    ```

1. In GitHub, send a pull request to merge with the base branch.

1. After your pull request is merged, make sure that your branch is deleted from the upstream repository.

### Reviewing a Pull Request

Anyone can review pull requests, we encourage others to review each other's work, however, only the code owners can approve a pull request.
Pull Requests often require several approvals from code owners, before being able to merge it.

## Your First Contribution

Unsure where to begin contributing? You can start by looking through good-first-issue and help-wanted issues:
"Good first issue" issues - issues that should only require a few lines of code, and a test or two.
"Help wanted" issues - issues that should be a bit more involved than good-first-issue issues.

Working on your first Pull Request? You can learn how from this *free* series, [How to Contribute to an Open Source Project on GitHub](https://app.egghead.io/playlists/how-to-contribute-to-an-open-source-project-on-github). If you prefer to read through some tutorials, visit <http://makeapullrequest.com/> and <http://www.firsttimersonly.com/>

At this point, you're ready to make your changes! Feel free to ask for help; everyone is a beginner at first 😺

If a maintainer asks you to "rebase" your PR, they're saying that a lot of code has changed and that you need to update your branch so it's easier to merge.

## Commit Guidelines
DeX uses [Semantic Release](https://semantic-release.gitbook.io/semantic-release/) to automate release and version management.
Therefore, commit messages should follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification.
Please review the specification summary for a quick start. DeX works wil the following commit types:
1. **feat**: a commit of the type *feat* introduces a new feature to the codebase. (this correlates with *MINOR* in Semantic Versioning).
1. **fix**: a commit of the type *fix* introduces a new feature to the codebase. (this correlates with *PATCH* in Semantic Versioning).
1. **perf**: a commit of the type *perf* introduces a new feature to the codebase. (this correlates with *PATCH* in Semantic Versioning).
1. **revert**: a commit of the type *revert* an existing commit from the codebase.
1. **docs**: a commit of the type *docs* updates anything documentation related. (this correlates with *PATCH* in Semantic Versioning).
1. **style**: a commit of the type *style* updates the styling related things in the codebase, such as semi-colons, quotes, trailing commas. (this correlates with *PATCH* in Semantic Versioning).
1. **chore**: a commit of the type *chore* is used when something does not fit in other categories, this will likely be a random task. (this correlates with *PATCH* in Semantic Versioning).
1. **refactor**: a commit of the type *refactor* updates existing functionality within the codebase without adding new features or fixing a bug. (this correlates with *PATCH* in Semantic Versioning).
1. **test**: a commit of the type *test* introduces new tests to the codebase. (this correlates with *PATCH* in Semantic Versioning).
1. **ci**: a commit of the type *ci* is used for anything workflow related, file structure changes, continuous integration, deployment and other processes. (this correlates with *PATCH* in Semantic Versioning).
1. **breaking**: a commit of the type *breaking* breaks existing projects when they update due to changed behavior. (this correlates with *MAJOR* in Semantic Versioning).

**Each Pull Request should have atleast one of the commits formatted in this formatted.** You are not required to commit each commit like such. It is recommend to commit often, and rebase to apply the commit standard to the final commit.

### Examples
#### Commit message with no body
```
docs: correct spelling of CHANGELOG
```

#### Commit message with scope
```
feat(homepage): add new homepage
```

### Commit message with footer
```
docs(coding-guidelines): add coding guidelines

there were no coding guidelines, this commit adds these guidelines with examples

fixes #ISSUE_NUMBER
```

You can commit multiple paragraphs with Git bash by doing the following:
```bash
git commit -m "docs(coding-guidelines): add coding guidelines" -m "there were no coding guidelines, this commit adds these guidelines with examples" -m "fixes #ISSUE_NUMBER"
```
