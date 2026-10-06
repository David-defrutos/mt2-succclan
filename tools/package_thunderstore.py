"""Build a Thunderstore ZIP without losing content directories in r2modman."""
import argparse
import json
import re
import zipfile
from pathlib import Path


def package(mod: Path, output: Path) -> Path:
    manifest = json.loads((mod / "manifest.json").read_text(encoding="utf-8-sig"))
    metadata = ["manifest.json", "README.md", "CHANGELOG.md", "LOCALIZATION.md",
                "icon.png", "LICENSE", "NOTICE.md", "ORIGINAL-ART.md", "AUDIOVISUAL.md"]
    dll = mod / "mt2_succclan.Plugin.dll"
    for name in metadata + [dll.name]:
        if not (mod / name).is_file():
            raise FileNotFoundError(mod / name)
    source = (mod / "src/Plugin.cs").read_text(encoding="utf-8-sig")
    registered = re.findall(r'"(json/[^"\r\n]+\.json)"', source)
    json_files = sorted((mod / "json").rglob("*.json"))
    assert registered and len(registered) == len(set(registered))
    assert set(registered) == {p.relative_to(mod).as_posix() for p in json_files}
    for path in json_files:
        data = json.loads(path.read_text(encoding="utf-8-sig"))
        for sprite in data.get("sprites", []):
            assert (mod / sprite["path"]).is_file(), (path, sprite["path"])
        for category, items in data.items():
            if isinstance(items, list):
                ids = [item["id"] for item in items if isinstance(item, dict) and isinstance(item.get("id"), str)]
                assert len(ids) == len(set(ids)), (path, category, "duplicate id")
    runtime = [dll] + json_files + sorted((mod / "textures").rglob("*.png"))
    output.mkdir(parents=True, exist_ok=True)
    target = output / f"{manifest['namespace']}-{manifest['name']}-{manifest['version_number']}-alpha.zip"
    with zipfile.ZipFile(target, "w", zipfile.ZIP_DEFLATED, compresslevel=7) as archive:
        for name in metadata:
            archive.write(mod / name, name)
        # A recognized plugins directory is copied as a unit by r2modman.
        # Root-level json/textures directories are recursively flattened instead.
        for path in runtime:
            archive.write(path, "plugins/" + path.relative_to(mod).as_posix())
    with zipfile.ZipFile(target) as archive:
        names = archive.namelist()
        assert archive.testzip() is None
        assert len(names) == len(set(names))
        assert "plugins/mt2_succclan.Plugin.dll" in names
        assert all("plugins/" + path in names for path in registered)
        assert not any(name.startswith(("json/", "textures/")) for name in names)
    print(f"JSON={len(json_files)} runtime files={len(runtime)} ZIP={target} bytes={target.stat().st_size}")
    return target


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--mod", type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument("--output", type=Path, required=True)
    args = parser.parse_args()
    package(args.mod, args.output)
