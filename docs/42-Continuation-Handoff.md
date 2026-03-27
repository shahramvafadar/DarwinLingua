# Continuation Handoff (Codex / Chat Reset)

## Purpose

This document captures the **minimum context needed to continue implementation in a new chat** without losing execution state.

Use it when:

- the active coding chat becomes too long
- tooling context is truncated
- you need to hand work to a new agent run

---

## Current Snapshot (as of 2026-03-27)

- Phase 1 is still in progress.
- Core app flows exist: home, CEFR browse, topic browse, search, word details, favorites, settings.
- Home now includes dashboard quick actions for search, topic browse, and favorites in addition to CEFR shortcuts.
- Catalog domain hardening is complete for Phase 1 aggregate invariants, uniqueness constraints, and topic-link relationship constraints.
- Learning-domain separation is now enforced at the persistence boundary: local user-state rows stay decoupled from catalog-content foreign keys and survive content-row deletion.
- Current MAUI user-facing labels and lexical metadata text are now routed through `AppStrings` resource catalogs for both English and German.
- Shared MAUI reusable controls currently include:
  - `WordListItemView`
  - `TopicListItemView`
  - `DetailSectionView`
  - `CefrQuickFilterView`
  - `ActionBlockView`
- Database initialization, seed workflows, localization setup, and transactional write service are implemented.
- Full local Windows checks now succeed: `dotnet restore`, `dotnet build`, and `dotnet test` on `DarwinLingua.slnx`.
- CI (`.github/workflows/ci.yml`) runs restore/build/test on non-MAUI projects and test projects.

---

## Recommended Next Implementation Slice

Focus next on Phase 1 items still marked in progress, especially **remaining UI/UX foundation work**.

Suggested scope:

1. Continue replacing remaining template-era UI surfaces with final product-oriented layouts.
2. Expand MAUI smoke coverage where practical around reusable controls and navigation-critical flows.
3. Keep all new user-facing labels localized through `AppStrings`.
4. Update `docs/04-Implementation-Backlog.md` and `README.md` to reflect actual completion state.

---

## New Chat Prompt Template

Use the following prompt to resume implementation in a fresh chat:

```text
Continue DarwinLingua Phase 1 implementation from the latest commit.

Context:
- Read and follow docs/04-Implementation-Backlog.md and docs/42-Continuation-Handoff.md first.
- Prioritize Phase 1 backlog items still marked in progress, especially remaining UI/UX foundation work.
- Keep all user-facing text localized via AppStrings resources for any newly added UI.
- After code changes, update backlog/docs status accurately.
- Run the full local Windows .NET checks after changes.

Delivery requirements:
- Make focused, production-quality changes.
- Run available checks.
- Commit changes.
- Provide a concise summary with exact file references and next step suggestions.
```

---

## Handoff Checklist (Before Ending a Chat)

- [ ] Backlog status markers are updated to match reality.
- [ ] README “Current Status” reflects newly completed capabilities.
- [ ] This handoff file is updated with date + latest next-step recommendation.
- [ ] A ready-to-paste “new chat prompt” is present and current.
