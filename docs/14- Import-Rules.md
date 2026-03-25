# 7) Import-Rules

```md
# Import Rules

## Import Purpose

The first import utility is intended to provide a simple and predictable way to insert structured learning content into the system database.

It is not intended to be a full content management platform in the first phase.

---

## First-Version Responsibilities

The initial import utility should:

- accept a JSON file
- parse the package
- validate required fields
- validate enum-like values such as CEFR level and part of speech
- validate language codes
- validate example structure
- check duplicates
- skip duplicates
- insert valid entries
- generate an import summary

---

## Duplicate Detection - First Version

The first version should use a conservative duplicate rule.

Recommended initial duplicate key:

- normalized German word
- part of speech
- primary CEFR level

This is not perfect, but it is acceptable for the first version.

Example duplicate candidates:

- Bahnhof + noun + A1
- laufen + verb + A1

If a duplicate is found:

- skip that entry
- log a warning
- continue processing

---

## Validation Rules

### File-Level Validation

- packageVersion is required
- packageId is required
- entries array is required
- entries array must not be empty

### Entry-Level Validation

- word is required
- language must be "de" in the first phase
- cefrLevel must be valid
- partOfSpeech must be valid
- meanings must not be empty
- examples must not be empty
- topics must not be empty

### Meaning Validation

- language is required
- text is required
- duplicate meaning languages inside the same entry should be rejected

### Example Validation

- baseText is required
- translations may be empty in the earliest version, but should normally be present
- duplicate translation languages inside the same example should be rejected

---

## Import Summary Output

The import process should output a summary such as:

- total entries found
- inserted entries count
- skipped duplicate count
- invalid entry count
- warning count

It should also include a list of warnings and errors.

---

## Recommended Import Modes for Future

Not required for v1, but useful later:

- validate only
- import with skip duplicates
- import with update existing
- dry run

The first version should only support:

- import with skip duplicates