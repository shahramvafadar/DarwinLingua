# Phase 1 Domain Cut

## Purpose

This document defines the exact domain cut for Phase 1 of Darwin Deutsch.

The full domain model has already been designed with long-term extensibility in mind. However, Phase 1 must remain disciplined and implementation-focused.

This document determines:

- which entities must be implemented in Phase 1
- which fields are required in Phase 1
- which fields should exist but may remain unused
- which entities are deferred to later phases
- which relationships must be implemented now
- which domain rules are active in Phase 1

The main goal of Phase 1 is to deliver a strong vocabulary-first learning product without overengineering.

---

# 1. Phase 1 Product Objective

Phase 1 is a multilingual German vocabulary application with:

- CEFR-based browsing
- topic-based browsing
- multilingual meanings
- one or two selected meaning languages
- example sentences
- audio playback through platform capabilities
- favorites
- basic user word state
- structured content import

Phase 1 is not intended to implement:

- advanced practice engine
- spaced repetition
- grammar engine
- migrant support resources
- web platform
- complex content management workflows

---

# 2. Phase 1 Domain Scope Summary

## 2.1 Must Be Implemented

The following entities must be implemented in Phase 1:

- WordEntry
- WordSense
- SenseTranslation
- ExampleSentence
- ExampleTranslation
- Topic
- TopicLocalization
- WordTopic
- Language
- UserLearningProfile
- UserFavoriteWord
- UserWordState
- ContentPackage
- ContentPackageEntry

## 2.2 May Be Implemented Only If Cheap and Clean

The following may be included only if they remain simple and do not slow Phase 1:

- PublicationStatus on major content entities
- CreatedAtUtc / UpdatedAtUtc on aggregates and key mutable entities
- RowVersion on major mutable aggregates
- ImportJob if import tooling needs it cleanly

## 2.3 Must Be Deferred

The following entities must not be implemented in Phase 1:

- UsageLabel
- UsageLabelLocalization
- WordSenseUsageLabel
- ContextLabel
- ContextLabelLocalization
- WordSenseContextLabel
- AudioAsset
- WordRelation
- Collocation
- CollocationTranslation
- GrammarHint
- UserRecentWord
- LearningList
- LearningListItem
- ReviewQueueItem
- PracticeSession
- PracticeSessionItem
- UserMistakePattern
- SupportResource
- SupportResourceCategory
- SupportResourceCategoryLocalization
- SupportResourceCategoryLink
- Address
- SupportResourceAddress
- SupportResourceTag

### Reason

These are valid future concepts, but Phase 1 should avoid carrying implementation burden for features not yet delivered.

---

# 3. Phase 1 Aggregates

## 3.1 Content Aggregates

Phase 1 should implement the following core content aggregates:

- WordEntry
- Topic
- Language
- ContentPackage

### Aggregate Composition

#### WordEntry aggregate should include:

- WordEntry
- WordSense
- SenseTranslation
- ExampleSentence
- ExampleTranslation
- WordTopic links

#### Topic aggregate should include:

- Topic
- TopicLocalization

#### ContentPackage aggregate should include:

- ContentPackage
- ContentPackageEntry

---

## 3.2 Learning Aggregates

Phase 1 should implement lightweight learning state only.

### Aggregates / Root-like entities

- UserLearningProfile
- UserFavoriteWord
- UserWordState

### Note

In early implementation, `UserFavoriteWord` and `UserWordState` may be treated as independent entities instead of rich aggregates.

That is acceptable for Phase 1.

---

# 4. Phase 1 WordEntry Design

## 4.1 Purpose

`WordEntry` is the central domain root for content in Phase 1.

It represents the German lexical entry shown to the user.

## 4.2 Required Fields

The following fields should be implemented and required in Phase 1:

- Id
- PublicId
- Lemma
- NormalizedLemma
- LanguageCode
- PrimaryCefrLevel
- PartOfSpeech
- PublicationStatus
- ContentSourceType
- CreatedAtUtc
- UpdatedAtUtc

## 4.3 Recommended Optional Fields

These should be present in the model if they can be added cleanly:

- Article
- PluralForm
- InfinitiveForm
- PronunciationIpa
- SyllableBreak
- SourceReference
- RowVersion

## 4.4 Deferred Fields

These fields should not be implemented in Phase 1:

- SearchLemma if it duplicates normalization without strong need
- ComparativeForm
- SuperlativeForm
- SeparablePrefix
- IsSeparableVerb
- IsReflexiveVerb
- IsIrregularVerb
- Gender as a separate advanced field if article already covers noun basics
- DifficultyScore
- FrequencyRank
- IsCommon
- NotesInternal
- PublishedAtUtc
- CreatedBy
- LastModifiedBy

### Reason

These are useful later, but not necessary to ship a clean first version.

---

# 5. Phase 1 WordSense Design

## 5.1 Purpose

`WordSense` allows a word to have one or more meanings.

This is essential even in Phase 1.

## 5.2 Required Fields

- Id
- WordEntryId
- SenseOrder
- IsPrimarySense
- PublicationStatus
- CreatedAtUtc
- UpdatedAtUtc

## 5.3 Optional Fields

- ShortDefinitionDe
- ShortGloss
- UsageNote
- GrammarNote
- RowVersion

## 5.4 Deferred Fields

- FullDefinitionDe
- PrimaryCefrLevel at sense level unless real content requires it early

### Recommendation

In Phase 1, keep sense-level CEFR out unless there is a clear content case.
Use word-level CEFR as the main browsing and filtering model.

---

# 6. Phase 1 SenseTranslation Design

## 6.1 Purpose

Stores meaning text in one target language for one specific sense.

## 6.2 Required Fields

- Id
- WordSenseId
- LanguageCode
- TranslationText
- IsPrimary
- CreatedAtUtc
- UpdatedAtUtc

## 6.3 Optional Fields

- Transliteration
- TranslationNote
- RowVersion

## 6.4 Deferred Rules

Support only one translation per target language per sense in Phase 1.

Do not support multiple competing translations per same language yet.

---

# 7. Phase 1 ExampleSentence Design

## 7.1 Purpose

Provides a German example sentence for a specific sense.

## 7.2 Required Fields

- Id
- WordSenseId
- SentenceOrder
- GermanText
- IsPrimaryExample
- CreatedAtUtc
- UpdatedAtUtc

## 7.3 Optional Fields

- CefrLevel
- ContextHint
- GrammarHint
- RowVersion

## 7.4 Deferred Fields

- NormalizedGermanText if search over examples is not needed in Phase 1
- UsageRegister
- SourceReference

### Recommendation

Do not add example search fields in Phase 1 unless there is a real implemented search use case.

---

# 8. Phase 1 ExampleTranslation Design

## 8.1 Purpose

Stores the translation of one example sentence in one target language.

## 8.2 Required Fields

- Id
- ExampleSentenceId
- LanguageCode
- TranslationText
- CreatedAtUtc
- UpdatedAtUtc

## 8.3 Optional Fields

- Transliteration
- TranslationNote
- RowVersion

---

# 9. Phase 1 Topic Design

## 9.1 Purpose

Supports topic-based browsing and filtering.

## 9.2 Topic Required Fields

- Id
- Key
- SortOrder
- IsSystem
- PublicationStatus
- CreatedAtUtc
- UpdatedAtUtc

## 9.3 Topic Optional Fields

- ParentTopicId
- IconKey
- ColorKey
- RowVersion

### Recommendation

Support `ParentTopicId` only if hierarchical topics are likely in Phase 1 data.
Otherwise it may remain null and unused.

---

# 10. Phase 1 TopicLocalization Design

## 10.1 Purpose

Provides topic display names in UI languages.

## 10.2 Required Fields

- Id
- TopicId
- LanguageCode
- DisplayName
- CreatedAtUtc
- UpdatedAtUtc

## 10.3 Optional Fields

- Description
- RowVersion

### Recommendation

DisplayName is enough for Phase 1.
Topic descriptions can wait.

---

# 11. Phase 1 WordTopic Design

## 11.1 Purpose

Links words to one or more topics.

## 11.2 Required Fields

- Id
- WordEntryId
- TopicId
- CreatedAtUtc

## 11.3 Optional Fields

- RelevanceScore
- IsPrimaryTopic

### Recommendation

If you want stricter and simpler Phase 1 behavior, include `IsPrimaryTopic` and skip `RelevanceScore`.

This makes topic display easier without introducing fuzzy scoring too early.

---

# 12. Phase 1 Language Design

## 12.1 Purpose

Represents supported languages for meanings and localizations.

## 12.2 Required Fields

- Id
- Code
- NameEnglish
- NativeName
- IsUiSupported
- IsMeaningSupported
- IsActive
- SortOrder
- CreatedAtUtc
- UpdatedAtUtc

## 12.3 Recommendation

Keep `Language` as controlled seed/reference data.

Phase 1 should not allow free runtime creation of languages by end users.

---

# 13. Phase 1 UserLearningProfile Design

## 13.1 Purpose

Stores user language preferences and lightweight learning preferences.

## 13.2 Required Fields

- Id
- UserId
- PreferredMeaningLanguage1
- UiLanguageCode
- CreatedAtUtc
- UpdatedAtUtc

## 13.3 Optional Fields

- NativeLanguageCode
- PreferredMeaningLanguage2
- PreferredAudioSpeed
- PreferredVoiceType
- DailyGoal
- CurrentCefrFocus
- RowVersion

## 13.4 Recommendation

If you want the app to support one or two selected meaning languages from the start, `PreferredMeaningLanguage2` should exist in Phase 1.

It may remain null for users who choose only one language.

---

# 14. Phase 1 UserFavoriteWord Design

## 14.1 Purpose

Stores user favorites.

## 14.2 Required Fields

- Id
- UserId
- WordEntryId
- CreatedAtUtc

## 14.3 Notes

This should remain very simple in Phase 1.

No additional metadata is necessary.

---

# 15. Phase 1 UserWordState Design

## 15.1 Purpose

Stores lightweight per-user state for a word.

This should support useful personalization without introducing a full review engine.

## 15.2 Required Fields

- Id
- UserId
- WordEntryId
- FirstViewedAtUtc
- LastViewedAtUtc
- ViewCount
- CreatedAtUtc
- UpdatedAtUtc

## 15.3 Recommended Optional Fields

- IsKnown
- IsDifficult
- LastPracticedAtUtc
- ConfidenceScore
- RowVersion

## 15.4 Deferred Fields

- IsHidden
- ManualNote

### Recommendation

For Phase 1, `IsKnown` and `IsDifficult` are enough if you want basic user marking.
Do not overcomplicate this entity.

---

# 16. Phase 1 ContentPackage Design

## 16.1 Purpose

Represents an imported content package.

## 16.2 Required Fields

- Id
- PackageId
- PackageVersion
- PackageName
- SourceType
- InputFileName
- ImportedAtUtc
- TotalEntries
- InsertedEntries
- SkippedEntries
- InvalidEntries
- WarningCount
- Status
- CreatedAtUtc
- UpdatedAtUtc

## 16.3 Optional Fields

- SourceReference
- Checksum
- ImportedBy
- Notes
- RowVersion

### Recommendation

Keep package tracking simple but real.
Do not treat import as a temporary script concern.

---

# 17. Phase 1 ContentPackageEntry Design

## 17.1 Purpose

Tracks one logical attempted content item from the import file.

## 17.2 Required Fields

- Id
- ContentPackageId
- RawLemma
- NormalizedLemma
- CefrLevel
- PartOfSpeech
- ProcessingStatus
- CreatedAtUtc
- UpdatedAtUtc

## 17.3 Optional Fields

- ExternalEntryKey
- ErrorCode
- ErrorMessage
- WarningMessage
- ImportedWordEntryId

### Recommendation

`ImportedWordEntryId` should be nullable because duplicates or invalid items may not produce a word record.

---

# 18. Relationships That Must Exist in Phase 1

The following relationships must be implemented in Phase 1:

- WordEntry -> WordSense
- WordSense -> SenseTranslation
- WordSense -> ExampleSentence
- ExampleSentence -> ExampleTranslation
- WordEntry -> WordTopic
- Topic -> WordTopic
- Topic -> TopicLocalization
- UserFavoriteWord -> WordEntry
- UserWordState -> WordEntry
- UserLearningProfile -> Language references
- ContentPackage -> ContentPackageEntry
- ContentPackageEntry -> WordEntry (nullable reference)

---

# 19. Relationships That Must Not Be Implemented Yet

The following relationships should remain design-only in Phase 1:

- WordSense -> UsageLabel
- WordSense -> ContextLabel
- WordEntry -> AudioAsset table
- WordEntry -> WordRelation
- WordEntry -> Collocation
- WordSense -> GrammarHint
- LearningList -> LearningListItem
- ReviewQueueItem -> WordEntry
- PracticeSession -> PracticeSessionItem
- SupportResource -> SupportResourceCategoryLink

---

# 20. Phase 1 Domain Rules

## 20.1 Active Word Minimum

An active word must have:

- a valid WordEntry
- at least one active sense
- at least one topic link
- at least one translation overall
- at least one example sentence overall

## 20.2 Translation Rule

Within one sense, duplicate translations for the same language are not allowed.

## 20.3 Example Translation Rule

Within one example sentence, duplicate translations for the same language are not allowed.

## 20.4 Topic Link Rule

A word cannot be linked to the same topic more than once.

## 20.5 User State Separation Rule

User-specific state must remain separate from content entities.

## 20.6 Import Safety Rule

Malformed entries must not break the entire import batch.

Duplicates must be skipped.

---

# 21. Phase 1 Uniqueness Constraints

The following uniqueness constraints should be implemented in Phase 1:

## 21.1 WordEntry

- NormalizedLemma + PartOfSpeech + PrimaryCefrLevel

## 21.2 WordSense

- WordEntryId + SenseOrder

## 21.3 SenseTranslation

- WordSenseId + LanguageCode

## 21.4 ExampleSentence

- WordSenseId + SentenceOrder

## 21.5 ExampleTranslation

- ExampleSentenceId + LanguageCode

## 21.6 Topic

- Key

## 21.7 TopicLocalization

- TopicId + LanguageCode

## 21.8 WordTopic

- WordEntryId + TopicId

## 21.9 Language

- Code

## 21.10 UserFavoriteWord

- UserId + WordEntryId

## 21.11 UserWordState

- UserId + WordEntryId

---

# 22. Phase 1 Reference Data

The following should exist as reference/seed data in Phase 1:

- Languages
- Topics
- Topic localizations
- allowed CEFR levels
- allowed part-of-speech values

### Recommendation

CEFR and part-of-speech may be implemented as enums or strongly controlled value objects rather than database lookup tables in Phase 1.

That is acceptable and simpler.

---

# 23. Phase 1 Search Scope

Search in Phase 1 should target:

- German lemma
- optionally normalized lemma
- optionally topic filters

Search should not initially depend on:

- example text indexing
- translation full-text indexing
- advanced ranking
- external search engines

### Recommendation

Keep search small and predictable.

---

# 24. Phase 1 Audio Decision

Audio in Phase 1 should use platform TTS rather than a stored audio domain model.

### Rule

Do not implement `AudioAsset` persistence in Phase 1.

### Reason

Audio playback is required, but audio storage infrastructure is not.

Platform TTS is sufficient for the first release.

---

# 25. Phase 1 Import Decision

The import tool in Phase 1 should:

- accept JSON input
- validate structure
- validate allowed values
- detect duplicates
- skip duplicates
- insert valid content
- log warnings and invalid items

### Rule

Do not implement content merge/update behavior in Phase 1.

Import should be insert-oriented with duplicate skipping only.

---

# 26. Phase 1 Persistence Guidance

Phase 1 persistence should favor simplicity.

### Recommendation

Use relational storage with explicit tables for:

- WordEntry
- WordSense
- SenseTranslation
- ExampleSentence
- ExampleTranslation
- Topic
- TopicLocalization
- WordTopic
- Language
- UserLearningProfile
- UserFavoriteWord
- UserWordState
- ContentPackage
- ContentPackageEntry

### Do Not Add Yet

- generalized metadata tables
- polymorphic ownership tables
- event store
- versioned content history

---

# 27. Phase 1 Application Simplicity Rules

To keep Phase 1 healthy, the implementation should avoid:

- generic repository abstractions that add no value
- speculative domain complexity for future features
- large admin workflows
- complex content versioning
- full editorial lifecycle logic
- cross-cutting abstractions created only for imagined future use

### Recommendation

The domain should be clean, but the implementation should remain pragmatic.

---

# 28. Phase 1 Success Shape

If Phase 1 is implemented correctly, the system will already support:

- multilingual vocabulary browsing
- topic browsing
- CEFR browsing
- word details with meanings and examples
- user preference for one or two meaning languages
- favorites
- lightweight per-word user state
- reliable structured content import

That is enough for a serious first production milestone.

---

# 29. Explicit Deferred List

The following should be documented as intentionally deferred, not forgotten:

- practice engine
- learning lists
- mistake tracking
- collocations
- lexical relations
- grammar hints
- audio asset persistence
- migrant support resource directory
- advanced admin workflows
- content editing UI
- web platform
- cloud sync

---

# 30. Final Phase 1 Cut Summary

## Implement Now

- multilingual vocabulary content model
- topics
- languages
- user preferences
- favorites
- basic user word state
- import package tracking

## Design Now, Implement Later

- advanced lexical metadata
- practice domain
- resource discovery domain
- explicit audio persistence
- advanced content workflow

## Do Not Blur the Boundary

The Phase 1 cut is deliberately smaller than the full domain.
That is a strength, not a limitation.