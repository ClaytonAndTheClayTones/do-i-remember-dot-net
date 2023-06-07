# Veho-Quality-Development-Kit (QDK)

An entity interaction and validation framework designed to enable client-side development for tests, seed scripts, cli tools and other internal operators.

This library stemmed from [a codex issue](https://github.com/veho-technologies/codex/issues/7) as we don’t currently have a way to generate test data needed to perform cross-system integration or e2e tests.  The QDK solves this issue by creating an interface that will allow consumers to generate data that can be used in our dev and staging environments.

Table of Contents
- [Veho-Quality-Development-Kit (QDK)](#veho-quality-development-kit-qdk)
- [Setup QDK in your project](#setup-qdk-in-your-project)
  - [Example](#example)
- [API Documentation](#api-documentation)
- [Logging](#logging)
- [Repo Overview](#repo-overview)
- [Setup for locally developing in the repository](#setup-for-locally-developing-in-the-repository)
  - [Local developing with NPM Link](#local-developing-with-npm-link)
- [Commit Standards](#commit-standards)
- [Releasing](#releasing)
- [Creating new Documentation](#creating-new-documentation)

# Setup QDK in your project
`npm i @veho/qdk --save-dev`

When running the QDK, you will need to have the `AWS_ACCESS_KEY_ID`, `AWS_REGION`, and `AWS_SECRET_ACCESS_KEY` exposed as environment variables.  Locally, this can be done using a tool like [awsume](https://github.com/veho-technologies/veho-platform#using-awsume-to-manage-current-environment-credentials) and in CircleCI, these can be sourced from [a shared context](https://app.circleci.com/settings/organization/github/veho-technologies/contexts) like `aws-platform`

When using specific QDK classes, the configuration will also ask for specific properties that will need to be provided when using that API.  

## Example

```typescript
import { ClustersQDK, FacilityId } from '@veho/qdk'
import * as config from './config'

const clustersQDK = new ClustersQDK({
  awsCognitoClientId: config.AWS_COGNITO_CLIENT_ID,
  awsRegion: config.AWS_REGION,
  testUserName: config.TEST_USERNAME,
  testPassword: config.TEST_PASSWORD,
  hermesGraphQLEndpoint: config.HERMES_GRAPHQL_URL,
  vehoApiEndpoint: config.VEHO_API_ENDPOINT,
  vehoClientApiKey: config.VEHO_CLIENT_API_KEY,
})

const clusters = await clustersQDK.createDeliveryClusters({
  facilityId: 'some-facility-id' as FacilityId,
  deliveryDate: DateTime.now().startOf('day').plus({ days: 1 }),
  numberOfOrders: 50,
})
```

# API Documentation
Read up on the auto-generated API documentation in [the docs folder](docs/README.md)

# Logging
By default, the QDK will use a log level of `warn`.  If you wish to view more or less logs that the QDK outputs, you can set the `QDK_LOG_LEVEL` environment variable using the log levels defined below.

| Log Level     | Priority      | 
| ------------- | ------------- |
| error         | 0             |
| warn          | 1             |
| http          | 3             |
| info          | 2             |
| verbose       | 4             |
| debug         | 5             |
| silly         | 7             |

If the log level is set to `error`, then only error messages will be displayed since `error` is the highest in priority.  If the log level is set to `verbose`, then everything including and above verbose (info, http, warn, and error) will be displayed.

# Repo Overview
The project has the following directory structure
```text
└── docs/ - QDK auto-generated documentation
└── qdk/ - Code for QDK Library
    ├── <DomainObject>/ - The domain object's QDK to house the code related to data generation for that domain object 
    ├── types/ - shared types, including error classes
    └── utils/ - helper code specific to the QDK
```

# Setup for locally developing in the repository
When working on the qdk, you'll want to follow these simple steps to get setup.

`nvm use`

`npm i`

## Local developing with NPM Link
When making changes to the library, you can test out the changes locally, in another project by using [npm link](https://docs.npmjs.com/cli/v9/commands/npm-link)

1. Clone this repo and start making changes
2. In root dir of this repo, simply type `npm link`.  this sets up the symlink behind the scenes
3. In your application that wants to use qdk, in the root dir, make sure you have qdk installed or install it using `npm i @veho/qdk --save-dev`
4. Type `npm link @veho/qdk` in the root directory of this project
5. Any changes you make to the local QDK should now be reflected in the project that's being tested.

# Commit Standards
In order to fully automate our release changelogs, we need to abide by our commit standards which are enforced via a husky commit-message hook.  These standards are based on [commitlint's](https://github.com/conventional-changelog/commitlint#what-is-commitlint) commit standards which forces each commit to have a prefix as one of the following:

```text
build:
chore:
ci:
docs:
feat:
fix:
perf:
refactor:
revert:
style:
test:
```

The following prefixes will trigger a release via ([semantic-release](https://github.com/semantic-release/semantic-release))
```text
feat:
fix:
perf:
```

`fix` will create a patch release, `feat` will create a minor release and `perf` will create a major release.

# Releasing
Whenever we make changes to the QDK, we should release a new version so that consumers can get the updated code.

We use [semantic-release](https://github.com/semantic-release/semantic-release) for our automated releases.  This package looks at recent commits since the last release and determines whether or not a new version needs to be released.  It determines to create a new release by parsing commit messages that have the prefix of `fix:`, `feat:` or `perf:`.  More information can be found in the [Commit message format section](https://github.com/semantic-release/semantic-release) in the semantic-release project.

To trigger a release, we'll need to do the following:
- Create a PR and merge that PR into main
- Wait for CircleCI's build to get to the approval step
- Approve the release by going to the pipeline in [CirclCI directly](https://app.circleci.com/pipelines/github/veho-technologies/veho-quality-development-kit) or by going to the pipeline via the `#_circleci` channel.
- `semantic-release` will fetch the latest git tags and check the commit messages from the last release to determine if a release is needed.
  -  If there are releasable commits, `semantic-release` will push out a new tag, publish the new version to npm and will also create a GitHub release page.

# Creating new Documentation
On post-commit, we use [typedoc](https://github.com/TypeStrong/typedoc) to auto-generate documentation of the QDK.  Whenever there are changes to the QDK, the docs will automatically be re-generated and added to your current commit.