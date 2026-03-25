# Darwin Lingua

Darwin Lingua is a modular language-learning platform built with .NET 10 and .NET MAUI.

It is designed as a **multi-product language ecosystem** where each end-user product targets a specific language while sharing a common architectural foundation.

The first active end-user product is:

- **Darwin Deutsch** — the German-learning product

The architecture is intentionally designed so that additional products can be introduced later, such as:

- Darwin English
- Darwin Arabic
- Darwin Turkish
- Darwin Persian

Each product can provide a focused learning experience for one target language while reusing the same core platform architecture.

---

## Platform Vision

Darwin Lingua is not intended to be just a single-language app.

It is designed as a reusable learning platform that supports:

- multiple language products
- cross-platform mobile and desktop delivery
- shared domain and application core
- local-first learning experience
- structured content growth through import packages
- future expansion to web, API, admin, practice, and resource-discovery modules

In Phase 1, the platform will ship only one active learner-facing product:

- **Darwin Deutsch**

---

## Darwin Deutsch

Darwin Deutsch is the first Darwin Lingua product.

It is a vocabulary-first German learning application focused on practical language learning for:

- immigrants
- newcomers
- foreign workers
- international learners
- users who need structured and useful German support in daily life

Phase 1 focuses on:

- CEFR-based vocabulary browsing
- topic-based vocabulary browsing
- multilingual meanings
- example sentences
- one or two selected meaning languages
- local-first behavior
- favorites
- lightweight user word state
- structured JSON-based content import

---

## Product Strategy

Darwin Lingua uses a two-level naming and product strategy:

### Platform / Solution Level

- **Darwin Lingua**

This is the ecosystem, repository, solution, and architectural umbrella.

### End-User Product Level

- **Darwin Deutsch**

This is the current user-facing application for German learning.

### Future Product Pattern

Future products should follow the same naming pattern:

- Darwin + target language name

Examples:

- Darwin English
- Darwin Arabic
- Darwin Turkish

This is why the architecture must not be hardcoded as German-only, even though Phase 1 implementation is German-focused.

---

## Core Principles

The platform is built around these principles:

- **Clean Architecture**
- **Modular Monolith**
- **Local-first Phase 1**
- **Vocabulary-first Phase 1**
- **Shared business core**
- **Clear bounded contexts**
- **Structured content import**
- **Future-ready multi-language product design**

---

## Technology Direction

Current intended stack:

- **.NET 10**
- **.NET MAUI**
- **Visual Studio 2026**
- **EF Core**
- **SQLite** for Phase 1 local storage

Planned future expansion may include:

- Web API
- Admin application
- Web application
- cloud sync scenarios
- richer practice features
- additional language products

---

## Repository Structure

Recommended repository structure:

```text
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
/src
/tests
/tools
/assets

The product is designed as a **local-first, vocabulary-first learning platform** focused on practical German usage for immigrants, newcomers, and other learners who need structured and useful support in real life.

The first production goal is a strong Phase 1 application with:

- CEFR-based vocabulary browsing
- topic-based vocabulary browsing
- multilingual meanings
- example sentences
- one or two selected meaning languages
- local-first behavior
- favorites
- lightweight user word state
- structured JSON-based content import

The long-term direction includes future expansion toward:

- practice and review features
- richer lexical intelligence
- web platform support
- content management workflows
- migrant-support resource discovery
- optional cloud and sync scenarios

---

## Why This Project Exists

Many German-learning products focus only on generic language drills. Darwin Deutsch is designed with a more practical goal:

- help users learn German vocabulary in structured levels from A1 to C2
- support multiple target meaning languages
- support real-life topic-based learning
- remain useful offline
- grow gradually through controlled content packages
- eventually support real-world newcomer needs beyond vocabulary

This project is intentionally designed to begin with a disciplined, high-value Phase 1 instead of an oversized first release.

---

## Core Product Direction

Darwin Deutsch is designed around these principles:

- **Vocabulary-first for Phase 1**
- **Local-first architecture**
- **Clean domain boundaries**
- **Shared business core across platforms**
- **Structured content growth**
- **Extensibility without premature complexity**

The project begins as a modular monolith and is intentionally not designed as microservices.

---

## Technology Direction

Current intended stack:

- **.NET 10**
- **.NET MAUI**
- **Visual Studio 2026**
- **EF Core**
- **SQLite** for Phase 1 local storage

Planned future expansion may include:

- Web API
- Web application
- Admin/content management application
- cloud synchronization scenarios

---

## Repository Documentation Structure

The documentation is organized under the `docs/` folder.

### Overview

- [Product Vision](docs/00-overview/01-Product-Vision.md)
- [Product Scope](docs/00-overview/02-Product-Scope.md)
- [Product Phases](docs/00-overview/03-Product-Phases.md)
- [Name Ideas](docs/00-overview/04-Name-Ideas.md)

### Content and Import

- [Content Strategy](docs/10-content/11-Content-Strategy.md)
- [AI Content Format](docs/10-content/12-AI-Content-Format.md)
- [Import Rules](docs/10-content/13-Import-Rules.md)
- [Import Workflow](docs/10-content/14-Import-Workflow.md)

### Domain

- [Domain Model](docs/20-domain/21-Domain-Model.md)
- [Domain Rules](docs/20-domain/22-Domain-Rules.md)
- [Entity Relationships](docs/20-domain/23-Entity-Relationships.md)
- [Phase 1 Domain Cut](docs/20-domain/24-Phase-1-Domain-Cut.md)
- [Bounded Contexts](docs/20-domain/25-Bounded-Contexts.md)

### Architecture

- [Solution Architecture](docs/30-architecture/31-Solution-Architecture.md)
- [Storage Strategy](docs/30-architecture/32-Storage-Strategy.md)
- [Offline Strategy](docs/30-architecture/33-Offline-Strategy.md)

### Implementation

- [Phase 1 Use Cases](docs/40-implementation/41-Phase-1-Use-Cases.md)

### Reference

- [Initial Topic Seed Ideas](docs/90-reference/91-Initial-Topic-Seed-Ideas.md)

---

## Recommended Reading Order

If you are new to the project, read the documentation in this order:

1. [Product Vision](docs/00-overview/01-Product-Vision.md)
2. [Product Scope](docs/00-overview/02-Product-Scope.md)
3. [Product Phases](docs/00-overview/03-Product-Phases.md)
4. [Content Strategy](docs/10-content/11-Content-Strategy.md)
5. [AI Content Format](docs/10-content/12-AI-Content-Format.md)
6. [Domain Model](docs/20-domain/21-Domain-Model.md)
7. [Domain Rules](docs/20-domain/22-Domain-Rules.md)
8. [Entity Relationships](docs/20-domain/23-Entity-Relationships.md)
9. [Phase 1 Domain Cut](docs/20-domain/24-Phase-1-Domain-Cut.md)
10. [Bounded Contexts](docs/20-domain/25-Bounded-Contexts.md)
11. [Solution Architecture](docs/30-architecture/31-Solution-Architecture.md)
12. [Storage Strategy](docs/30-architecture/32-Storage-Strategy.md)
13. [Offline Strategy](docs/30-architecture/33-Offline-Strategy.md)
14. [Import Workflow](docs/10-content/14-Import-Workflow.md)
15. [Phase 1 Use Cases](docs/40-implementation/41-Phase-1-Use-Cases.md)

---

## Phase 1 Summary

Phase 1 is intentionally smaller than the long-term product vision.

### Phase 1 includes

- German vocabulary entries
- CEFR levels
- topic-based categorization
- multilingual meanings
- example sentences
- one or two selected meaning languages
- platform-based text-to-speech usage
- favorites
- lightweight user word state
- local SQLite storage
- structured JSON import workflow
- local-first/offline-first behavior

### Phase 1 does not include

- full practice engine
- spaced repetition
- collocations and lexical relations
- grammar engine
- support-resource discovery
- cloud sync
- web application
- admin editing workflows
- content merging during import

This boundary is deliberate and should be preserved.

---

## Domain Overview

The project domain is divided into bounded contexts.

### Active in Phase 1

- **Content Catalog**
- **Learning Profile**
- **Content Operations**
- **Localization Support**

### Designed but deferred

- **Practice**
- **Resource Discovery**

For details, see [Bounded Contexts](docs/20-domain/25-Bounded-Contexts.md).

---

## Architecture Overview

Darwin Deutsch should be implemented as a **modular monolith** with clean internal boundaries.

### Phase 1 projects

- `DarwinDeutsch.Domain`
- `DarwinDeutsch.Application`
- `DarwinDeutsch.Infrastructure`
- `DarwinDeutsch.Maui`
- `DarwinDeutsch.ImportTool`

### Planned future projects

- `DarwinDeutsch.WebApi`
- `DarwinDeutsch.Web`
- `DarwinDeutsch.Admin`

For details, see [Solution Architecture](docs/30-architecture/31-Solution-Architecture.md).

---

## Storage Overview

Phase 1 storage strategy:

- **SQLite**
- **EF Core**
- one local relational database per app installation
- imported content packages
- locally stored user preferences and lightweight user state
- no server dependency for core learning behavior

For details, see:

- [Storage Strategy](docs/30-architecture/32-Storage-Strategy.md)
- [Offline Strategy](docs/30-architecture/33-Offline-Strategy.md)

---

## Content Import Overview

Darwin Deutsch content is expected to grow gradually through structured content packages.

The official Phase 1 content import approach is:

- JSON-based input
- validated structure
- controlled language/topic references
- duplicate detection
- skip duplicates
- insert valid content
- import reporting and traceability

For details, see:

- [AI Content Format](docs/10-content/12-AI-Content-Format.md)
- [Import Rules](docs/10-content/13-Import-Rules.md)
- [Import Workflow](docs/10-content/14-Import-Workflow.md)

---

## Essential Phase 1 Use Cases

The most important Phase 1 use cases are:

- browse words by CEFR
- browse words by topic
- search words by German lemma
- get word details
- update meaning language preferences
- toggle favorite
- track word viewed
- mark word as known
- mark word as difficult
- import content package
- initialize database
- seed reference data
- ensure user learning profile exists

For full details, see [Phase 1 Use Cases](docs/40-implementation/41-Phase-1-Use-Cases.md).

---

## Recommended Solution Folder Direction

A recommended future solution structure is:

```text
/README.md
/docs
/src
  /DarwinDeutsch.Domain
  /DarwinDeutsch.Application
  /DarwinDeutsch.Infrastructure
  /DarwinDeutsch.Maui
  /DarwinDeutsch.ImportTool
/tests
  /DarwinDeutsch.Domain.Tests
  /DarwinDeutsch.Application.Tests
  /DarwinDeutsch.Infrastructure.Tests