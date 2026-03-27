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
- Shared MAUI reusable controls currently include:
  - `WordListItemView`
  - `DetailSectionView`
- Database initialization, seed workflows, localization setup, and transactional write service are implemented.
- CI (`.github/workflows/ci.yml`) runs restore/build/test on non-MAUI projects and test projects.

---

## Known Environment Limitation in This Container

- `dotnet` CLI is not available in the current execution environment (`dotnet: command not found`).
- As a result, build/test verification cannot be executed here until a .NET SDK environment is provided.

---

## Recommended Next Implementation Slice

Focus next on completing the reusable component backlog item for **filters/actions** in MAUI screens.

Suggested scope:

1. Extract a reusable CEFR quick-filter/action block from `HomePage`.
2. Reuse the same component in other screens where applicable.
3. Keep all user-facing labels localized through `AppStrings`.
4. Add/adjust MAUI tests or smoke checks where practical.
5. Update `docs/04-Implementation-Backlog.md` and `README.md` to reflect actual completion state.

---

## New Chat Prompt Template

Use the following prompt to resume implementation in a fresh chat:

```text
Continue DarwinLingua Phase 1 implementation from the latest commit.

Context:
- Read and follow docs/04-Implementation-Backlog.md and docs/42-Continuation-Handoff.md first.
- Prioritize completing reusable MAUI filter/action components (the backlog item currently marked in progress).
- Keep all user-facing text localized via AppStrings resources.
- After code changes, update backlog/docs status accurately.
- If the environment lacks dotnet SDK, document the limitation clearly and still complete code/doc updates.

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
