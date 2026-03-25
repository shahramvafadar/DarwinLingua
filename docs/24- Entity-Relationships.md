# Entity Relationships

## Purpose

This document defines the main relationships between domain entities in Darwin Deutsch.

It clarifies:

- cardinalities
- parent-child relationships
- many-to-many links
- optional vs required relationships
- recommended cascade behavior
- implementation notes for relational persistence

This document complements:

- `20-Domain-Model.md`
- `21-Domain-Rules.md`

---

# 1. Content Context Relationships

## 1.1 WordEntry -> WordSense

### Relationship

- one `WordEntry` has many `WordSense`
- one `WordSense` belongs to exactly one `WordEntry`

### Cardinality

- `WordEntry (1) -> (many) WordSense`
- `WordSense (many) -> (1) WordEntry`

### Required

- required on child side

### Recommended Delete Behavior

- cascade in controlled maintenance scenarios
- restrict physical delete in normal production usage

### Notes

This is a true parent-child content relationship.

---

## 1.2 WordSense -> SenseTranslation

### Relationship

- one `WordSense` has many `SenseTranslation`
- one `SenseTranslation` belongs to exactly one `WordSense`

### Cardinality

- `WordSense (1) -> (many) SenseTranslation`

### Required

- required on child side

### Unique Rule

- unique by `WordSenseId + LanguageCode`

---

## 1.3 WordSense -> ExampleSentence

### Relationship

- one `WordSense` has many `ExampleSentence`
- one `ExampleSentence` belongs to exactly one `WordSense`

### Cardinality

- `WordSense (1) -> (many) ExampleSentence`

### Required

- required on child side

### Notes

This relationship is important because examples should be meaning-aware.

---

## 1.4 ExampleSentence -> ExampleTranslation

### Relationship

- one `ExampleSentence` has many `ExampleTranslation`
- one `ExampleTranslation` belongs to exactly one `ExampleSentence`

### Cardinality

- `ExampleSentence (1) -> (many) ExampleTranslation`

### Required

- required on child side

### Unique Rule

- unique by `ExampleSentenceId + LanguageCode`

---

## 1.5 WordEntry -> WordTopic

### Relationship

- one `WordEntry` has many `WordTopic`
- one `WordTopic` belongs to exactly one `WordEntry`

### Cardinality

- `WordEntry (1) -> (many) WordTopic`

### Notes

This is part of a many-to-many relationship between words and topics.

---

## 1.6 Topic -> WordTopic

### Relationship

- one `Topic` has many `WordTopic`
- one `WordTopic` belongs to exactly one `Topic`

### Cardinality

- `Topic (1) -> (many) WordTopic`

### Unique Rule

- unique by `WordEntryId + TopicId`

### Resulting Many-to-Many

- `WordEntry (many) <-> (many) Topic`

---

## 1.7 Topic -> TopicLocalization

### Relationship

- one `Topic` has many `TopicLocalization`
- one `TopicLocalization` belongs to exactly one `Topic`

### Cardinality

- `Topic (1) -> (many) TopicLocalization`

### Unique Rule

- unique by `TopicId + LanguageCode`

---

## 1.8 WordSense -> WordSenseUsageLabel

### Relationship

- one `WordSense` has many `WordSenseUsageLabel`
- one `WordSenseUsageLabel` belongs to one `WordSense`

---

## 1.9 UsageLabel -> WordSenseUsageLabel

### Relationship

- one `UsageLabel` has many `WordSenseUsageLabel`
- one `WordSenseUsageLabel` belongs to one `UsageLabel`

### Resulting Many-to-Many

- `WordSense (many) <-> (many) UsageLabel`

### Unique Rule

- unique by `WordSenseId + UsageLabelId`

---

## 1.10 UsageLabel -> UsageLabelLocalization

### Relationship

- one `UsageLabel` has many `UsageLabelLocalization`
- one `UsageLabelLocalization` belongs to one `UsageLabel`

### Unique Rule

- unique by `UsageLabelId + LanguageCode`

---

## 1.11 WordSense -> WordSenseContextLabel

### Relationship

- one `WordSense` has many `WordSenseContextLabel`
- one `WordSenseContextLabel` belongs to one `WordSense`

---

## 1.12 ContextLabel -> WordSenseContextLabel

### Relationship

- one `ContextLabel` has many `WordSenseContextLabel`
- one `WordSenseContextLabel` belongs to one `ContextLabel`

### Resulting Many-to-Many

- `WordSense (many) <-> (many) ContextLabel`

### Unique Rule

- unique by `WordSenseId + ContextLabelId`

---

## 1.13 ContextLabel -> ContextLabelLocalization

### Relationship

- one `ContextLabel` has many `ContextLabelLocalization`
- one `ContextLabelLocalization` belongs to one `ContextLabel`

### Unique Rule

- unique by `ContextLabelId + LanguageCode`

---

## 1.14 Language -> SenseTranslation

### Relationship

- one `Language` may be referenced by many `SenseTranslation`
- each `SenseTranslation` references one target language code

### Cardinality

- `Language (1) -> (many) SenseTranslation`

### Notes

In database design, this may be enforced by FK to `Language.Code` or through application validation depending on modeling choice.

---

## 1.15 Language -> ExampleTranslation

### Relationship

- one `Language` may be referenced by many `ExampleTranslation`
- each `ExampleTranslation` references one target language code

---

## 1.16 Language -> TopicLocalization / UsageLabelLocalization / ContextLabelLocalization

Each localization entity references exactly one language.

---

## 1.17 WordEntry -> AudioAsset

### Relationship

- one `WordEntry` may have zero or many `AudioAsset`
- one `AudioAsset` may reference one owner

### Cardinality

- `WordEntry (1) -> (0..many) AudioAsset`

### Notes

This is polymorphic ownership if `AudioAsset` can also point to `ExampleSentence`.

If polymorphic mapping becomes awkward in EF Core, split into:

- WordAudioAsset
- ExampleAudioAsset

That may be cleaner in implementation.

---

## 1.18 ExampleSentence -> AudioAsset

### Relationship

- one `ExampleSentence` may have zero or many `AudioAsset`

### Notes

Same polymorphic design warning applies here.

---

## 1.19 WordEntry -> WordRelation

### Relationship

- one `WordEntry` may be source of many `WordRelation`
- one `WordEntry` may be target of many `WordRelation`

### Result

Self-referencing relationship between words.

### Notes

This is useful for synonyms, antonyms, related words, and word family links.

---

## 1.20 WordEntry -> Collocation

### Relationship

- one `WordEntry` may have many `Collocation`
- one `Collocation` belongs to one `WordEntry`

---

## 1.21 Collocation -> CollocationTranslation

### Relationship

- one `Collocation` has many `CollocationTranslation`
- one `CollocationTranslation` belongs to one `Collocation`

---

## 1.22 WordSense -> GrammarHint

### Relationship

- one `WordSense` may have zero or many `GrammarHint`
- one `GrammarHint` belongs to one `WordSense`

---

# 2. Content Import Relationships

## 2.1 ContentPackage -> ContentPackageEntry

### Relationship

- one `ContentPackage` has many `ContentPackageEntry`
- one `ContentPackageEntry` belongs to one `ContentPackage`

### Cardinality

- `ContentPackage (1) -> (many) ContentPackageEntry`

---

## 2.2 ContentPackage -> ImportJob

### Relationship

- one `ContentPackage` may have one or more technical import jobs over time
- one `ImportJob` belongs to one `ContentPackage`

### Notes

This is useful if the same package is retried after failure.

---

## 2.3 ContentPackageEntry -> WordEntry

### Relationship

- one `ContentPackageEntry` may reference zero or one imported `WordEntry`
- one `WordEntry` may be associated with one source package entry in the first version

### Notes

If duplicates are skipped, some package entries will have no linked `WordEntry`.

---

# 3. Learning Context Relationships

## 3.1 UserLearningProfile -> Language

### Relationship

A user learning profile references preferred languages.

### References

- NativeLanguageCode -> Language
- PreferredMeaningLanguage1 -> Language
- PreferredMeaningLanguage2 -> Language
- UiLanguageCode -> Language

### Notes

These are references, not ownership.

---

## 3.2 UserFavoriteWord -> WordEntry

### Relationship

- one user may favorite many words
- one word may be favorited by many users

### Modeled Through

`UserFavoriteWord`

### Resulting Many-to-Many

- `User (many) <-> (many) WordEntry`

---

## 3.3 UserWordState -> WordEntry

### Relationship

- one user may have one state record per word
- one word may be linked to many user state records

### Recommended Unique Rule

- `UserId + WordEntryId`

---

## 3.4 UserRecentWord -> WordEntry

### Relationship

- one user may have many recent view records
- one word may appear in many user recents

### Notes

Depending on implementation, this may be a log table rather than a strict domain aggregate child.

---

## 3.5 LearningList -> LearningListItem

### Relationship

- one `LearningList` has many `LearningListItem`
- one `LearningListItem` belongs to one `LearningList`

---

## 3.6 LearningListItem -> WordEntry

### Relationship

- one `LearningListItem` references one `WordEntry`
- one `WordEntry` may appear in many learning lists

---

# 4. Practice Context Relationships

## 4.1 ReviewQueueItem -> WordEntry

### Relationship

- one review queue item points to one word
- one word may appear in many users' review queues

### Recommended Unique Rule

- `UserId + WordEntryId + SourceType`

Depending on practice design, `SourceType` may or may not be needed.

---

## 4.2 PracticeSession -> PracticeSessionItem

### Relationship

- one `PracticeSession` has many `PracticeSessionItem`
- one `PracticeSessionItem` belongs to one `PracticeSession`

---

## 4.3 PracticeSessionItem -> WordEntry

### Relationship

- one session item references one word
- one word may appear in many sessions

---

## 4.4 UserMistakePattern -> WordEntry

### Relationship

- one mistake pattern points to one word
- one word may have many mistake patterns across users

### Recommended Unique Rule

- `UserId + WordEntryId + MistakeType`

---

# 5. Resource Discovery Relationships

## 5.1 SupportResource -> SupportResourceCategoryLink

### Relationship

- one `SupportResource` has many category links
- one category link belongs to one resource

---

## 5.2 SupportResourceCategory -> SupportResourceCategoryLink

### Relationship

- one `SupportResourceCategory` has many category links
- one category link belongs to one category

### Resulting Many-to-Many

- `SupportResource (many) <-> (many) SupportResourceCategory`

---

## 5.3 SupportResourceCategory -> SupportResourceCategoryLocalization

### Relationship

- one `SupportResourceCategory` has many localizations
- one localization belongs to one category

---

## 5.4 SupportResource -> SupportResourceAddress

### Relationship

- one `SupportResource` may have one or more linked addresses
- one link belongs to one resource

---

## 5.5 Address -> SupportResourceAddress

### Relationship

- one reusable address may be linked to one or more resources
- one link belongs to one address

### Notes

If address reuse is unnecessary, this can later be simplified into direct address fields on `SupportResource`.

---

## 5.6 SupportResource -> SupportResourceTag

### Relationship

- one `SupportResource` may have many tags
- one tag belongs to one resource

---

# 6. Required Relationship Summary

The following child relationships are required:

- WordSense -> WordEntry
- SenseTranslation -> WordSense
- ExampleSentence -> WordSense
- ExampleTranslation -> ExampleSentence
- TopicLocalization -> Topic
- UsageLabelLocalization -> UsageLabel
- ContextLabelLocalization -> ContextLabel
- ContentPackageEntry -> ContentPackage
- LearningListItem -> LearningList
- PracticeSessionItem -> PracticeSession

---

# 7. Many-to-Many Relationship Summary

The following are explicit many-to-many relationships through link entities:

- WordEntry <-> Topic via WordTopic
- WordSense <-> UsageLabel via WordSenseUsageLabel
- WordSense <-> ContextLabel via WordSenseContextLabel
- SupportResource <-> SupportResourceCategory via SupportResourceCategoryLink
- User <-> WordEntry via UserFavoriteWord
- User <-> WordEntry via UserWordState
- User <-> WordEntry via review/practice entities

---

# 8. Optional Relationship Summary

The following are optional in early versions:

- WordEntry -> AudioAsset
- ExampleSentence -> AudioAsset
- WordEntry -> WordRelation
- WordEntry -> Collocation
- WordSense -> GrammarHint
- SupportResource -> SupportResourceTag
- SupportResource -> SupportResourceAddress

---

# 9. Recommended Physical Database Notes

## 9.1 Use Explicit Link Entities

Even where EF Core supports implicit many-to-many, explicit link entities are recommended.

Why:

- future metadata support
- easier auditing
- better extensibility
- more predictable migrations

Examples:

- WordTopic
- WordSenseUsageLabel
- WordSenseContextLabel
- SupportResourceCategoryLink

---

## 9.2 Prefer Stable Reference Tables

The following should be stable reference tables:

- Language
- Topic
- UsageLabel
- ContextLabel
- SupportResourceCategory

---

## 9.3 Consider Separate Audio Tables if Needed

If polymorphic audio ownership complicates persistence, replace `AudioAsset` with separate concrete tables.

This is cleaner than forcing weak polymorphic persistence too early.

---

# 10. Recommended Phase-1 Relationship Set

The first implementation can safely start with this reduced relationship graph:

- WordEntry -> WordSense
- WordSense -> SenseTranslation
- WordSense -> ExampleSentence
- ExampleSentence -> ExampleTranslation
- WordEntry -> WordTopic -> Topic
- Topic -> TopicLocalization
- UserLearningProfile -> Language
- UserFavoriteWord -> WordEntry
- UserWordState -> WordEntry
- ContentPackage -> ContentPackageEntry

This is enough for a serious first release.

---

# 11. ASCII Relationship View

```text
WordEntry
 ├── WordSense
 │    ├── SenseTranslation
 │    ├── ExampleSentence
 │    │    └── ExampleTranslation
 │    ├── WordSenseUsageLabel ─── UsageLabel ─── UsageLabelLocalization
 │    ├── WordSenseContextLabel ─── ContextLabel ─── ContextLabelLocalization
 │    └── GrammarHint
 ├── WordTopic ─── Topic ─── TopicLocalization
 ├── WordRelation (self-reference to WordEntry)
 ├── Collocation ─── CollocationTranslation
 └── AudioAsset (optional)

Language
 ├── SenseTranslation
 ├── ExampleTranslation
 ├── TopicLocalization
 ├── UsageLabelLocalization
 ├── ContextLabelLocalization
 └── UserLearningProfile references

ContentPackage
 ├── ContentPackageEntry
 └── ImportJob

UserLearningProfile
UserFavoriteWord ─── WordEntry
UserWordState ─── WordEntry
UserRecentWord ─── WordEntry

LearningList
 └── LearningListItem ─── WordEntry

PracticeSession
 └── PracticeSessionItem ─── WordEntry

ReviewQueueItem ─── WordEntry

SupportResource
 ├── SupportResourceCategoryLink ─── SupportResourceCategory ─── Localization
 ├── SupportResourceAddress ─── Address
 └── SupportResourceTag