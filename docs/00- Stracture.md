/README.md
/LICENSE.md
/NOTICE.md
/.editorconfig
/.gitattributes
/.gitignore
/Directory.Build.props
/Directory.Build.targets
/Directory.Packages.props
/global.json

/docs
  /00-overview
    01-Product-Vision.md
    02-Product-Scope.md
    03-Product-Phases.md
    04-Name-Ideas.md

  /10-content
    11-Content-Strategy.md
    12-AI-Content-Format.md
    13-Import-Rules.md
    14-Import-Workflow.md

  /20-domain
    21-Domain-Model.md
    22-Domain-Rules.md
    23-Entity-Relationships.md
    24-Phase-1-Domain-Cut.md
    25-Bounded-Contexts.md

  /30-architecture
    31-Solution-Architecture.md
    32-Storage-Strategy.md
    33-Offline-Strategy.md

  /40-implementation
    41-Phase-1-Use-Cases.md

  /90-reference
    91-Initial-Topic-Seed-Ideas.md

/src
  /BuildingBlocks
    /DarwinLingua.SharedKernel
    /DarwinLingua.Contracts

  /Modules
    /Catalog
      /DarwinLingua.Catalog.Domain
      /DarwinLingua.Catalog.Application
      /DarwinLingua.Catalog.Infrastructure

    /Learning
      /DarwinLingua.Learning.Domain
      /DarwinLingua.Learning.Application
      /DarwinLingua.Learning.Infrastructure

    /ContentOps
      /DarwinLingua.ContentOps.Domain
      /DarwinLingua.ContentOps.Application
      /DarwinLingua.ContentOps.Infrastructure

    /Localization
      /DarwinLingua.Localization.Domain
      /DarwinLingua.Localization.Application
      /DarwinLingua.Localization.Infrastructure

    /ResourceDirectory
      /DarwinLingua.ResourceDirectory.Domain
      /DarwinLingua.ResourceDirectory.Application
      /DarwinLingua.ResourceDirectory.Infrastructure

    /Practice
      /DarwinLingua.Practice.Domain
      /DarwinLingua.Practice.Application
      /DarwinLingua.Practice.Infrastructure

  /Apps
    /DarwinLingua.AppHost
    /DarwinDeutsch.Maui
    /DarwinLingua.ImportTool

  /Presentation
    /DarwinLingua.WebApi
    /DarwinLingua.Admin
    /DarwinLingua.Web

/tests
  /BuildingBlocks
    /DarwinLingua.SharedKernel.Tests

  /Modules
    /Catalog
      /DarwinLingua.Catalog.Domain.Tests
      /DarwinLingua.Catalog.Application.Tests
      /DarwinLingua.Catalog.Infrastructure.Tests

    /Learning
      /DarwinLingua.Learning.Domain.Tests
      /DarwinLingua.Learning.Application.Tests
      /DarwinLingua.Learning.Infrastructure.Tests

    /ContentOps
      /DarwinLingua.ContentOps.Domain.Tests
      /DarwinLingua.ContentOps.Application.Tests
      /DarwinLingua.ContentOps.Infrastructure.Tests

    /Localization
      /DarwinLingua.Localization.Domain.Tests
      /DarwinLingua.Localization.Application.Tests
      /DarwinLingua.Localization.Infrastructure.Tests

  /Apps
    /DarwinDeutsch.Maui.Tests

/tools
  /scripts
  /samples
    /content-packages

/assets
  /branding
  /icons
  /screenshots