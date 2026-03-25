# AI Content File Format

## Format Choice

The primary import format should be JSON.

Reasons:

- structured and strict
- easy to validate
- easy to parse in .NET
- suitable for AI-generated content
- good for future import tooling

YAML may be supported later as an optional authoring format, but JSON should be the official import format.

---

## File-Level Structure

Each file should contain:

- package metadata
- supported meaning languages
- word entries

---

## Suggested File Name Examples

- a1_basic_home_pack_01.json
- a1_travel_pack_fa_en.json
- b1_workplace_words_2026_01.json
- mixed_doctor_topic_pack.json

---

## Root JSON Shape

```json
{
  "packageVersion": "1.0",
  "packageId": "a1-travel-fa-en-001",
  "packageName": "A1 Travel Starter Pack",
  "source": "AI-assisted",
  "defaultMeaningLanguages": ["fa", "en"],
  "entries": []
}