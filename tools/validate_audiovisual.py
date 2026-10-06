"""Validate audiovisual references and optionally preserve gameplay against a backup.

Reads only; no Unity rendering or audio playback is performed.
"""
import argparse
import base64
import json
import re
import struct
from pathlib import Path

PRESENTATION_KEYS = {
    'attack_vfx', 'impact_vfx', 'death_vfx', 'projectile_vfx', 'applied_vfx',
    'applied_to_self_vfx', 'triggered_vfx', 'added_vfx', 'removed_vfx',
    'affected_vfx', 'persistent_vfx', 'applied_sfx', 'triggered_sfx',
}


def walk(value):
    if isinstance(value, dict):
        for key, child in value.items():
            yield key, child
            yield from walk(child)
    elif isinstance(value, list):
        for child in value:
            yield from walk(child)


def gameplay(value):
    if isinstance(value, dict):
        return {k: gameplay(v) for k, v in value.items() if k not in PRESENTATION_KEYS}
    if isinstance(value, list):
        return [gameplay(v) for v in value]
    return value


def catalog_keys(raw):
    """Read path/stem and GUID pairs from Unity Addressables' key data."""
    keys = {}
    for match in re.finditer(rb'\x05 ([0-9a-f]{32})', raw):
        end = match.start()
        candidates = []
        for size in range(1, min(512, end - 4) + 1):
            if struct.unpack_from('<I', raw, end - size - 4)[0] != size:
                continue
            try:
                name = raw[end - size:end].decode('utf-8')
            except UnicodeDecodeError:
                continue
            if name.isprintable():
                candidates.append(name)
        if candidates:
            keys[max(candidates, key=len)] = match.group(1).decode('ascii')
    return keys


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--mod', type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument('--baseline', type=Path)
    parser.add_argument('--catalog', type=Path)
    args = parser.parse_args()
    paths = list(dict.fromkeys(re.findall(r'"(json/[^"\r\n]+\.json)"',
                                        (args.mod / 'src/Plugin.cs').read_text(encoding='utf-8-sig'))))
    if not paths:
        raise ValueError('No registered JSON paths')
    data = {p: json.loads((args.mod / p).read_text(encoding='utf-8-sig')) for p in paths}
    visual = [v for d in data.values() for v in d.get('vfxs', [])]
    ids = {v['id'] for v in visual}
    if len(ids) != len(visual):
        raise ValueError('Duplicate VFX IDs')
    references = []
    for file, document in data.items():
        for key, value in walk(document):
            if key in PRESENTATION_KEYS and isinstance(value, str) and value.startswith('@SuccFX'):
                if value[1:] not in ids:
                    raise ValueError(f'{file}: unresolved {value}')
                references.append(value)
    characters = [e for d in data.values() for e in d.get('characters', [])]
    rooms = [e for d in data.values() for e in d.get('room_modifiers', [])]
    for character in characters:
        if not all(character.get(key) for key in ('attack_vfx', 'impact_vfx', 'death_vfx')):
            raise ValueError('Incomplete character presentation: ' + character['id'])
    for room in rooms:
        if not room.get('triggered_vfx'):
            raise ValueError('Missing room pulse: ' + room['id'])
    unused = ids - {r[1:] for r in references}
    if unused:
        raise ValueError('Unused VFX definitions: ' + ', '.join(sorted(unused)))
    for vfx in visual:
        asset = vfx['vfx_left']
        if not re.fullmatch(r'[0-9a-f]{32}', asset['asset_guid']):
            raise ValueError('Invalid GUID: ' + vfx['id'])
    if args.catalog:
        catalog = json.loads(args.catalog.read_text(encoding='utf-8-sig'))
        keys = catalog_keys(base64.b64decode(catalog['m_KeyDataString']))
        paths_in_catalog = set(catalog['m_InternalIds'])
        for vfx in visual:
            asset = vfx['vfx_left']
            path = asset['asset_name']
            stem = Path(path).stem
            guid = keys.get(path)
            if guid is None:
                same_stem = [p for p in paths_in_catalog if Path(p).stem == stem]
                if len(same_stem) == 1:
                    guid = keys.get(stem)
            if path not in paths_in_catalog or guid != asset['asset_guid']:
                raise ValueError('Path/GUID mismatch in current catalog: ' + vfx['id'])
    baseline_count = 0
    if args.baseline:
        for relative in json.loads((args.baseline / 'registered.json').read_text(encoding='utf-8')):
            before = json.loads((args.baseline / relative).read_text(encoding='utf-8-sig'))
            if gameplay(before) != gameplay(data[relative]):
                raise ValueError('Gameplay/text changed: ' + relative)
            baseline_count += 1
    print(json.dumps({'registered_json': len(paths), 'vfx_definitions': len(visual),
                      'resolved_references': len(references), 'characters': len(characters),
                      'rooms': len(rooms), 'unchanged_gameplay_text_json': baseline_count,
                      'current_catalog_presence_checked': bool(args.catalog)}))


if __name__ == '__main__':
    main()
