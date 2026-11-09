# Localization

SuccClan 0.1.1 includes 253 localized fields in each of the ten languages supported by Monster Train 2: English, French, German, Brazilian Portuguese, Russian, Spanish, Simplified Chinese, Traditional Chinese, Japanese and Korean.

## Scope and review

The English and existing Simplified Chinese texts are preserved. The other eight language dictionaries have been completed, including names, champion paths, effects, essences, artifacts, rooms, equipment, active abilities, status tooltips and in-play room text.

The new translations are a first pass. They have not been reviewed by native speakers or inspected in all languages in game. Shared game and dependency keywords are also affected by the translations supplied by Monster Train 2 and Conductor.

Feedback: [GitHub issues](https://github.com/David-defrutos/mt2-succclan/issues) or [Monster Train Discord modding channel](https://discord.com/channels/336546996779483136/1377778943674810368).

## Maintenance

`tools/localization/names.tsv` contains the terminology and names. `templates.tsv` contains eight translations per effect template; its IDs refer to `english-unique.json`. Column order: Spanish, French, German, Brazilian Portuguese, Russian, Traditional Chinese, Japanese, Korean. Square-bracket game tokens are protected and inserted after translation. Proper names Knightmare, Vrolikai and Oolioddroo remain unchanged.

Run `python tools/localization/apply_localization.py` to regenerate these translations after editing the tables. Existing Simplified Chinese text is retained. The script rejects English strings without a translation and creates a local backup before changing a JSON file. If English source text changes, update the source list and its templates together.

Run `python tools/localization/validate_localization.py 32c0c3e` to compare against 0.1.0. It checks every language, protected variables, balanced markup, fixed quantities and unchanged gameplay data. Existing English and Simplified Chinese must remain unchanged. Japanese singular counters and the wording for doubling are normalized when comparing fixed quantities. An English indefinite Spark means one card.

Change the baseline commit when auditing a later translation-only update. Backups and validation output are local artifacts and are ignored by Git. Authoring tools are excluded from the Thunderstore ZIP.

## 0.1.1 validation

- 61 JSON files parsed; 253 fields in every language; no missing translations.
- No mismatches in protected variables or markup in the eight completed translations.
- Fixed quantities verified with language-aware counter normalization.
- IDs, effect references, targets, stats and other gameplay configuration match 0.1.0.
- Local DLL build: zero errors and zero warnings. The only code metadata change is version 0.1.1.


## Current maintenance — 2026-11-09

The 0.1.1 counts and translation-only baseline above are historical. The current JSON contains 265 localized dictionaries in ten languages. Card text follows the workspace editorial guide: concise actions, native trigger and ability labels, explicit exceptional conditions, correct targets, and dynamic effect/upgrade quantities where supported. The November revision changes localization fields only, verified against the installed 0.2.1 gameplay data.

The legacy TSV/source-list generator describes the first translation pass and is not synchronized with all later JSON edits. Do not regenerate current content from those tables or use the old 0.1.0 baseline as a current validator. Review the current JSON directly, check all language keys, token parity and balanced markup, and compare non-localization data with the pre-edit runtime or a current snapshot. Linguistic review focuses on English and Spanish; structural validation of the other translations does not replace native-speaker review or in-game layout checks.

<!-- 2026-11-09-2043||codex-succclan-targetless||repos/MT2/variaciones-mias/mt2-succclan/LOCALIZATION.md||Actualizar limites de mantenimiento y validador historico;265 campos actuales y revision editorial solo texto -->
