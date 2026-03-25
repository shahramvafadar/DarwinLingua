# Domain Rules

## Purpose

This document defines the core domain rules for Darwin Deutsch.

The goal is to prevent inconsistent implementation by making the following explicit:

- aggregate boundaries
- ownership rules
- lifecycle rules
- uniqueness rules
- validation rules
- deletion and archival rules
- import constraints
- user data separation rules

This document should be used together with `20-Domain-Model.md`.

---

# 1. Aggregate Boundaries

## 1.1 Content Context

### Aggregate Root: WordEntry

The `WordEntry` aggregate is the central content aggregate.

It owns:

- WordSense
- ExampleSentence
- SenseTranslation
- ExampleTranslation
- lexical metadata directly attached to the word

### Design Rule

A `WordSense` cannot exist without a `WordEntry`.

An `ExampleSentence` cannot exist without a `WordSense`.

A `SenseTranslation` cannot exist without a `WordSense`.

An `ExampleTranslation` cannot exist without an `ExampleSentence`.

### Implication

When loading a word in editing or import workflows, the application should treat the word and its nested content as one logical content unit.

---

## 1.2 Topic Aggregate

### Aggregate Root: Topic

The `Topic` aggregate owns:

- TopicLocalization

The many-to-many link `WordTopic` is not owned by `Topic` in the same way as localization. It is a link entity between `WordEntry` and `Topic`.

---

## 1.3 UsageLabel Aggregate

### Aggregate Root: UsageLabel

The `UsageLabel` aggregate owns:

- UsageLabelLocalization

The linking entity `WordSenseUsageLabel` belongs to association management, not to label-localization ownership.

---

## 1.4 ContextLabel Aggregate

### Aggregate Root: ContextLabel

The `ContextLabel` aggregate owns:

- ContextLabelLocalization

The linking entity `WordSenseContextLabel` is an association entity, not part of localization ownership.

---

## 1.5 Language Aggregate

### Aggregate Root: Language

`Language` is a stable reference aggregate.

It should not own other entities directly in the first design.

It is referenced from:

- SenseTranslation
- ExampleTranslation
- TopicLocalization
- UsageLabelLocalization
- ContextLabelLocalization
- UserLearningProfile
- future localized entities

---

## 1.6 ContentPackage Aggregate

### Aggregate Root: ContentPackage

The `ContentPackage` aggregate owns:

- ContentPackageEntry

The optional `ImportJob` may reference `ContentPackage` but does not have to be owned by it in the strict DDD sense.

---

## 1.7 Learning Context

### Aggregate Root: UserLearningProfile

This aggregate represents user-specific learning preferences.

It does not own all user learning history.

It is separate from:

- UserFavoriteWord
- UserWordState
- UserRecentWord
- LearningList

These may be separate aggregates or application-managed entities depending on implementation scale.

### Design Rule

User-specific learning data must never be embedded inside content aggregates.

---

## 1.8 LearningList Aggregate

### Aggregate Root: LearningList

The `LearningList` aggregate owns:

- LearningListItem

---

## 1.9 Practice Context

### Aggregate Root: ReviewQueueItem

Each `ReviewQueueItem` is user-specific and independent.

### Aggregate Root: PracticeSession

The `PracticeSession` aggregate owns:

- PracticeSessionItem

---

## 1.10 Resource Discovery Context

### Aggregate Root: SupportResource

The `SupportResource` aggregate may own or coordinate:

- SupportResourceCategoryLink
- SupportResourceAddress
- SupportResourceTag

The reusable `Address` entity may be modeled as a shared reference entity instead of a strict child entity, depending on implementation style.

### Aggregate Root: SupportResourceCategory

The `SupportResourceCategory` aggregate owns:

- SupportResourceCategoryLocalization

---

# 2. Ownership Rules

## 2.1 WordEntry Owns WordSense

A sense is semantically meaningless without a parent word entry.

### Rule

- `WordSense.WordEntryId` is required
- orphan senses are not allowed

---

## 2.2 WordSense Owns ExampleSentence

Examples belong to a specific sense, not only to the raw word.

### Rule

- `ExampleSentence.WordSenseId` is required
- orphan examples are not allowed

### Reason

This preserves semantic correctness for multi-meaning words.

---

## 2.3 WordSense Owns SenseTranslation

Meaning translations must be attached to a sense.

### Rule

- `SenseTranslation.WordSenseId` is required
- storing free translations directly on `WordEntry` is not allowed

---

## 2.4 ExampleSentence Owns ExampleTranslation

Example translations must be attached to a specific example.

### Rule

- `ExampleTranslation.ExampleSentenceId` is required

---

## 2.5 Topic Owns TopicLocalization

A topic can exist without localization temporarily, but a localization cannot exist without a topic.

---

## 2.6 Language is Reference Data

Language records should be treated as controlled reference data.

### Rule

Application logic should reject translations pointing to unsupported or inactive language codes.

---

# 3. Identity Rules

## 3.1 Internal Identity

All entities should use internal primary keys.

Recommended:

- database numeric IDs or GUID/ULID-based IDs
- plus optional public identifiers where external exposure is needed

### Examples

- WordEntry.Id -> internal
- WordEntry.PublicId -> external safe identifier
- SupportResource.PublicId -> external safe identifier

---

## 3.2 Stable External Identity

Where content may be exposed via API or URLs, use stable public identifiers.

### Rule

Never expose raw technical database details as a long-term public contract if public IDs are available.

---

# 4. Uniqueness Rules

## 4.1 WordEntry Uniqueness

Initial recommended uniqueness rule:

- `NormalizedLemma`
- `PartOfSpeech`
- `PrimaryCefrLevel`

### Warning

This is acceptable for early content import, but not a perfect linguistic identity model.

### Future Improvement

A more advanced model may later consider sense-aware or frequency-aware uniqueness.

---

## 4.2 WordSense Order Uniqueness

Within one `WordEntry`, `SenseOrder` should be unique.

### Rule

No duplicate sense positions under the same word.

---

## 4.3 SenseTranslation Uniqueness

Within one `WordSense`, there must be at most one translation for the same target language.

### Unique Key

- `WordSenseId`
- `LanguageCode`

---

## 4.4 ExampleSentence Order Uniqueness

Within one `WordSense`, `SentenceOrder` should be unique.

---

## 4.5 ExampleTranslation Uniqueness

Within one `ExampleSentence`, there must be at most one translation for the same target language.

### Unique Key

- `ExampleSentenceId`
- `LanguageCode`

---

## 4.6 Topic Key Uniqueness

`Topic.Key` must be globally unique.

Examples:

- travel
- work
- doctor

No duplicate topic keys are allowed.

---

## 4.7 Topic Localization Uniqueness

At most one localization per topic per language.

### Unique Key

- `TopicId`
- `LanguageCode`

---

## 4.8 UsageLabel Key Uniqueness

`UsageLabel.Key` must be globally unique.

---

## 4.9 ContextLabel Key Uniqueness

`ContextLabel.Key` must be globally unique.

---

## 4.10 WordTopic Link Uniqueness

The same word should not be linked to the same topic more than once.

### Unique Key

- `WordEntryId`
- `TopicId`

---

## 4.11 WordSenseUsageLabel Link Uniqueness

### Unique Key

- `WordSenseId`
- `UsageLabelId`

---

## 4.12 WordSenseContextLabel Link Uniqueness

### Unique Key

- `WordSenseId`
- `ContextLabelId`

---

## 4.13 Language Code Uniqueness

`Language.Code` must be globally unique.

Examples:

- de
- en
- fa

---

## 4.14 User Favorite Uniqueness

A user can favorite a word only once.

### Unique Key

- `UserId`
- `WordEntryId`

---

## 4.15 Learning List Item Uniqueness

A word should not appear twice in the same learning list unless the product explicitly supports duplicates.

### Recommended Unique Key

- `LearningListId`
- `WordEntryId`

---

# 5. Required vs Optional Data Rules

## 5.1 WordEntry Required Fields

The following should be required in phase 1:

- Lemma
- NormalizedLemma
- LanguageCode
- PrimaryCefrLevel
- PartOfSpeech
- PublicationStatus
- ContentSourceType

### Initial Restriction

In phase 1, `LanguageCode` should always be `de` for content words.

---

## 5.2 WordEntry Optional Fields

Optional in early versions:

- Article
- PluralForm
- InfinitiveForm
- ComparativeForm
- SuperlativeForm
- PronunciationIpa
- SyllableBreak
- FrequencyRank
- DifficultyScore

These fields are valuable but should not block import if absent.

---

## 5.3 WordSense Required Fields

Recommended required fields:

- WordEntryId
- SenseOrder

The following should be strongly recommended but may be optional in earliest imports:

- at least one translation
- at least one example sentence

### Product Rule

For released content, every active sense should normally have:

- at least one meaning translation
- at least one example sentence

---

## 5.4 ExampleSentence Required Fields

Required:

- WordSenseId
- GermanText
- SentenceOrder

Optional but recommended:

- CefrLevel
- ContextHint
- GrammarHint

---

## 5.5 Topic Link Requirement

Every active `WordEntry` should have at least one topic link.

This is important for topic-based browsing.

---

# 6. Validation Rules

## 6.1 Lemma Validation

`Lemma` must not be empty.

`NormalizedLemma` must be generated according to a stable normalization rule.

### Recommended Normalization Rules

- trim whitespace
- convert to lowercase
- collapse repeated spaces
- normalize Unicode where appropriate

The exact implementation should be stable and centralized.

---

## 6.2 CEFR Validation

Only the allowed CEFR values are accepted.

Any other value must be rejected during validation/import.

---

## 6.3 Part of Speech Validation

Only supported part-of-speech values are accepted.

Unknown text values should not be silently stored as free text.

---

## 6.4 Language Code Validation

All language codes must exist in the `Language` reference set and must be active for the intended use case.

Example:
A translation language must exist and be enabled as a meaning language.

---

## 6.5 Example Quality Rules

At the domain policy level:

- example text must not be empty
- example text should not be identical to the lemma only
- example text should be meaningful German content

These quality rules may be enforced partly at application/import level instead of strict domain constructors.

---

## 6.6 Translation Quality Rules

Translations must not be empty.

Whitespace-only translations are invalid.

---

# 7. Lifecycle Rules

## 7.1 PublicationStatus Usage

Content entities that are user-visible should use `PublicationStatus` instead of hard deletion in normal operations.

Recommended values:

- Draft
- Active
- Archived
- Deprecated

### Rule

Only `Active` content should be shown to end users by default.

---

## 7.2 Draft Content

Draft content may exist without full completeness.

Example:
A draft word may exist before all translations or examples are finalized.

### Rule

Draft content is editable and not yet user-visible.

---

## 7.3 Active Content

Active content is user-visible and should meet minimum quality requirements.

### Minimum expectation for an active word

- valid WordEntry
- at least one active sense
- at least one translation in a supported meaning language
- at least one topic
- at least one example sentence overall

---

## 7.4 Archived Content

Archived content is not shown in normal user browsing but is preserved for history, traceability, and references.

---

## 7.5 Deprecated Content

Deprecated content may remain technically available for compatibility, but should not be shown in new learning flows.

This status is useful for future migration or content replacement scenarios.

---

# 8. Delete Rules

## 8.1 Hard Delete is Restricted for Content

Hard deletion of content should be rare.

### Rule

User-facing content should generally not be physically deleted if it may already be referenced by:

- user favorites
- user word states
- review history
- import history

---

## 8.2 Archive Instead of Delete

If content should be removed from user view, set `PublicationStatus` to `Archived` or `Deprecated`.

---

## 8.3 Child Delete Cascades

Within a content aggregate, child entities may be cascade-deleted when the parent is physically deleted in controlled maintenance scenarios.

Examples:

- deleting a `WordEntry` may delete its senses, examples, and translations
- deleting a `Topic` may delete its localizations

But this should not be normal production behavior for active content.

---

# 9. Import Rules at Domain Level

## 9.1 Import Must Not Bypass Domain Integrity

The import pipeline must not insert partial malformed structures that violate domain rules.

Example:
It must not insert a translation without a valid parent sense.

---

## 9.2 Duplicate Handling

The initial rule is conservative:

- if duplicate word identity is detected, skip the entry
- log the result
- continue the batch

### Rule

Import must be batch-safe and predictable.

---

## 9.3 Import Source Traceability

Every imported content record should preserve enough source metadata to know where it came from.

Recommended:

- ContentSourceType
- SourceReference
- ContentPackage reference

---

## 9.4 Import Idempotency Consideration

The first version may not be fully idempotent, but the design should move toward predictable repeat imports later.

---

# 10. User Data Separation Rules

## 10.1 Content vs User State

Content entities represent shared global learning material.

User entities represent per-user progress and personalization.

### Rule

Never store:

- favorite count
- known state
- difficulty state
- review data

inside content entities directly.

---

## 10.2 Per-User Preferences

Meaning language preferences and UI language preferences belong to `UserLearningProfile`, not to the content domain.

---

# 11. Search Support Rules

## 11.1 Search Fields Must Be Stable

Fields like `NormalizedLemma` and `SearchLemma` must be kept consistent with the visible lemma.

If the lemma changes, normalization/search fields must be updated in the same transaction.

---

## 11.2 Search Across Languages

In phase 1, search may primarily target German lemma and topic metadata.

Later, search may also target:

- meaning translations
- examples
- support resources

The domain should support this without restructuring.

---

# 12. Resource Domain Rules

## 12.1 SupportResource Minimum Fields

An active support resource should normally have:

- Name
- ResourceType
- at least one category
- at least one location or usable address description

---

## 12.2 Resource Verification

The field `VerifiedAtUtc` should be optional but useful.

A missing verification date does not block storage, but verified records may later be prioritized in UI.

---

# 13. Audit Rules

## 13.1 Important Mutable Aggregates Require Audit Fields

At minimum, important aggregates should track:

- CreatedAtUtc
- UpdatedAtUtc

For larger admin workflows, also track:

- CreatedBy
- LastModifiedBy

---

## 13.2 Concurrency Control

Mutable content aggregates should support optimistic concurrency using `RowVersion` or equivalent.

This is especially important for future admin editing and batch imports.

---

# 14. Domain Event Candidates

Not required in phase 1, but the design should anticipate these events:

- WordEntryCreated
- WordEntryPublished
- ContentPackageImported
- UserFavoritedWord
- ReviewQueueItemScheduled
- SupportResourceVerified

These may remain application-layer events in early versions.

---

# 15. Phase-1 Hard Rules Summary

The following should be treated as hard implementation rules for phase 1:

- every word must have a stable normalized lemma
- every active word must have at least one topic
- every active word must have at least one sense
- every active sense must have at least one translation
- every active word should have at least one example sentence overall
- duplicate translation languages per sense are not allowed
- duplicate translation languages per example are not allowed
- duplicate topic links are not allowed
- content and user state must remain separate
- import must skip duplicates and continue