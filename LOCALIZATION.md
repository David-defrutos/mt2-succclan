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
