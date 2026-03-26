# Product Scope

## Purpose

This document defines the practical scope of Darwin Deutsch, with special focus on the Phase 1 delivery boundary.

It exists to answer:

- what this product is responsible for now
- what must be implemented in Phase 1
- what must remain out of scope for Phase 1
- what platform, localization, and delivery constraints are active from day one

This document should be read together with:

- `01-Product-Vision.md`
- `03-Product-Phases.md`
- `21-Early-Product-Decisions.md`
- `25-Phase-1-Domain-Cut.md`

---

## 1. Product Boundary

### 1.1 Platform Boundary

`Darwin Lingua` is the platform and repository umbrella.

`Darwin Deutsch` is the first learner-facing product on that platform.

### 1.2 Product Boundary

Darwin Deutsch is a practical German-learning product focused on structured vocabulary learning for real-life usage.

It is not a generic social platform, not a broad AI tutor, and not a full migrant-service portal in Phase 1.

---

## 2. Phase 1 In Scope

Phase 1 must deliver a serious, usable, local-first vocabulary product.

### 2.1 User Features In Scope

- browse words by CEFR level
- browse words by topic
- search by German lemma
- view word details
- show multilingual meanings
- show example sentences
- play pronunciation through platform text-to-speech
- save favorite words
- store lightweight user word state
- support one or two selected meaning languages at the same time

### 2.2 Delivery Scope In Phase 1

- MAUI learner application
- local database initialization
- reference data seeding
- offline-capable core experience
- structured JSON content import through a separate import host/tool

### 2.3 Data Scope In Phase 1

- German source vocabulary
- controlled CEFR values
- controlled topic keys
- multilingual meaning translations
- example sentences and example translations
- user preferences and lightweight local user state

---

## 3. Phase 1 Out of Scope

The following must remain outside the Phase 1 implementation boundary:

- spaced repetition
- flashcard engine
- review sessions
- quiz engine
- grammar engine
- collocations and lexical relations
- support-resource directory
- social/community features
- public user-generated content
- cloud sync
- user accounts
- server-driven content updates
- web application
- public web API
- admin editing workflows
- import merge/update behavior

That boundary is deliberate and should not be blurred.

---

## 4. Platform Scope

### 4.1 Active Phase 1 Platforms

- Android
- iOS
- Windows

### 4.2 Planned Later Platforms

- Web

The architecture must support future web reuse, but the first production priority is the MAUI application.

---

## 5. Localization Scope

Localization is not optional and must start in Phase 1.

### 5.1 UI Language Scope

The user interface must support these UI languages from the start:

- English
- German

### 5.2 Localization Rules

- all user-visible UI strings must come from localization resources
- no hard-coded user-facing strings should remain in screens or view models
- the default UI language should follow the device language where supported
- the user must be able to override the UI language in settings

### 5.3 Meaning Language Scope

The content model must support multiple meaning languages.

Phase 1 may begin with a limited subset of meaning languages in real content packs, but the platform must support:

- one selected meaning language
- two selected meaning languages at the same time

---

## 6. Technical Scope

### 6.1 Storage Scope

For the MAUI/local-first Phase 1 product, SQLite is the correct storage choice.

If later web, API, admin, or shared cloud scenarios require a stronger server database, PostgreSQL is a good default candidate for that server-side part of the platform.

### 6.2 Import Scope

Phase 1 import is:

- JSON-based
- validation-driven
- insert-oriented
- duplicate-aware
- offline-capable

It is not:

- merge-capable
- admin-driven content management
- dynamic uncontrolled taxonomy creation

### 6.3 Quality Scope

Phase 1 must not be treated as a throwaway prototype.

The implementation is expected to follow the engineering standards defined in:

- `35-Engineering-Standards.md`

---

## 7. UX Scope

UI/UX is a first-class concern from the beginning.

Phase 1 must not ship as a purely technical demo.

Minimum UX expectations:

- clear browsing flow
- readable multilingual presentation
- consistent localization behavior
- responsive layouts
- accessible interaction patterns
- stable offline behavior

---

## 8. Scope Guardrails

When deciding whether a feature belongs in Phase 1, use these questions:

- does it directly improve vocabulary discovery or understanding?
- does it support local-first reliability?
- does it preserve architectural clarity?
- can it be implemented without dragging in future-phase complexity?

If the answer is no, it probably does not belong in Phase 1.

---

## 9. Final Scope Summary

Phase 1 is a multilingual, local-first, vocabulary-first German learning product with:

- structured content
- stable import workflow
- multilingual UI support
- English and German UI localization
- clear architectural boundaries

It is intentionally not yet a full learning ecosystem.
