# Documento generado el 2026-11-07-1206
"""SuccClan: validate, install in existing Default, or prepare a local ZIP."""
import argparse
import hashlib
import importlib.util
import json
import os
from pathlib import Path
import shutil
import subprocess
import sys
import uuid
import zipfile
from datetime import datetime
from zoneinfo import ZoneInfo

REPO = Path(__file__).resolve().parents[1]
WORKSPACE = Path(r"D:\Juegos\MT2_mod")
INSTALL = Path(r"C:\Users\david\AppData\Roaming\Thunderstore Mod Manager\DataFolder\MonsterTrain2\profiles\Default\BepInEx\plugins\David-SuccClan_Custom")
MANAGED = Path(r"C:\Juegos\Steam\steamapps\common\Monster Train 2\MonsterTrain2_Data\Managed")
META = ['manifest.json', 'README.md', 'CHANGELOG.md', 'LOCALIZATION.md', 'icon.png', 'LICENSE', 'NOTICE.md', 'ORIGINAL-ART.md', 'AUDIOVISUAL.md']
DLL = 'mt2_succclan.Plugin.dll'

def digest(data):
    return hashlib.sha256(data).hexdigest()

def run(*args):
    subprocess.run([str(a) for a in args], cwd=REPO, check=True)

def select_default_runtime(candidates=None):
    # Inspect only executable files; never edit manager settings or enable .old DLLs.
    candidates = candidates or [INSTALL.parent / 'frutos-SuccClan', INSTALL.parent / 'David-SuccClan_Custom']
    active = [path for path in candidates if (path / DLL).is_file()]
    if len(active) != 1:
        raise RuntimeError('Cannot identify exactly one existing executable SuccClan copy in Default; nothing was enabled or changed.')
    return active[0]

def closed():
    result = subprocess.run(['powershell.exe', '-NoProfile', '-NonInteractive', '-Command',
        "if (Get-Process -Name MonsterTrain2 -ErrorAction SilentlyContinue) { exit 1 }"], check=False)
    if result.returncode:
        raise RuntimeError('Close Monster Train 2 before installing. Default was not changed.')

def log(path, reason):
    registry = WORKSPACE / 'docs/registro/script-succclan-develop.py.md'
    registry.parent.mkdir(parents=True, exist_ok=True)
    stamp = datetime.now(ZoneInfo('Europe/Madrid')).strftime('%Y-%m-%d-%H%M')
    with registry.open('a', encoding='utf-8') as stream:
        stream.write(f'\n{stamp}||script-succclan-develop.py||{path}||{reason}\n')

def runtime():
    files = {name: (REPO / name) for name in META}
    files.update({p.relative_to(REPO).as_posix(): p for folder in ('json', 'textures')
                  for p in (REPO / folder).rglob('*') if p.is_file() and p.suffix in ('.json', '.png')})
    return files

def input_hash():
    files = dict(runtime())
    files.update({p.relative_to(REPO).as_posix(): p for folder in ('src', 'tools', 'tests')
                  for p in (REPO / folder).rglob('*') if p.is_file()
                  and not {'obj', 'bin', '__pycache__'}.intersection(p.relative_to(REPO).parts)
                  and p.suffix in ('.cs', '.csproj', '.py', '.Config', '.config', '.json')})
    entries = [(name, digest(path.read_bytes())) for name, path in sorted(files.items())]
    return digest(json.dumps(entries).encode())

def validate():
    if REPO == INSTALL or INSTALL in REPO.parents:
        raise RuntimeError('Run this tool from the external development repository.')
    manifest = json.loads((REPO / 'manifest.json').read_text(encoding='utf-8-sig'))
    version = manifest['version_number']
    info = (REPO / 'tools/local-build/MyPluginInfo.cs').read_text(encoding='utf-8')
    if f'PLUGIN_VERSION="{version}"' not in info:
        raise RuntimeError('Local MyPluginInfo.cs does not match manifest version.')
    stamp = datetime.now(ZoneInfo('Europe/Madrid')).strftime('%Y-%m-%d-%H%M%S')
    output = REPO / '.build' / (stamp + '-' + uuid.uuid4().hex[:8])
    output.mkdir(parents=True, exist_ok=False)
    before = input_hash()
    config = REPO / 'tools/local-build/NuGet.Config'
    project = REPO / 'tools/local-build/SuccClan.Local.csproj'
    run('dotnet', 'restore', project, '--configfile', config, '-p:NuGetAudit=false')
    run('dotnet', 'build', project, '-c', 'Release', '--no-restore', '--output', output / 'dll')
    run(sys.executable, REPO / 'tests/validate_content.py')
    run(sys.executable, REPO / 'tests/test_workflow.py')
    for path in ('tests/behavior/Tests.csproj', 'tests/shadow-preview/Preview.csproj', 'tests/native-api/ApiCheck.csproj'):
        run('dotnet', 'restore', REPO / path, '--configfile', config, '-p:NuGetAudit=false')
        command = ['dotnet', 'run', '--project', REPO / path, '-c', 'Release', '--no-restore']
        if 'native-api' in path:
            command += ['--', output / 'dll' / DLL, MANAGED / 'Assembly-CSharp.dll', INSTALL.parent / 'Conductor-Conductor/Conductor.dll']
        run(*command)
    run(sys.executable, REPO / 'tools/validate_audiovisual.py', '--mod', REPO)
    if input_hash() != before:
        raise RuntimeError('Inputs changed during validation. Repeat validation before installing/packaging.')
    commit = subprocess.check_output(['git', '-c', 'safe.directory=' + str(REPO), '-C', str(REPO), 'rev-parse', 'HEAD'], text=True).strip()
    dirty = subprocess.check_output(['git', '-c', 'safe.directory=' + str(REPO), '-C', str(REPO), 'status', '--porcelain'], text=True).strip()
    result = {'document_generated_at': stamp, 'repo': str(REPO), 'commit': commit,
              'dirty': bool(dirty), 'input_sha256': before, 'version': version,
              'built_dll': str(output / 'dll' / DLL),
              'dll_sha256': digest((output / 'dll' / DLL).read_bytes()),
              'action': 'validate', 'installed': False, 'published': False}
    report = output / 'validation.json'
    report.write_text(json.dumps(result, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    log(report, 'Validar fuentes externas y suites; no instalar ni publicar')
    print('Validated:', report, flush=True)
    return output, result

def contained(base, relative):
    path = (base / relative).resolve()
    if not path.is_relative_to(base.resolve()) or path == base.resolve():
        raise RuntimeError('Unsafe path: ' + relative)
    return path

def install(output, result):
    closed()
    if input_hash() != result['input_sha256']:
        raise RuntimeError('Source changed after validation.')
    planned = {name: path.read_bytes() for name, path in runtime().items()}
    planned[DLL] = Path(result['built_dll']).read_bytes()
    state = REPO / '.build/default-installed-files.json'
    state_before = (state.read_bytes(), state.stat().st_mtime_ns) if state.exists() else None
    previous = state if state.exists() else REPO / 'tools/default-runtime-files.json'
    owned = set(json.loads(previous.read_text(encoding='utf-8'))['files'])
    snapshot = {}
    for name in sorted(set(planned) | owned):
        path = contained(INSTALL, name)
        snapshot[name] = (path.read_bytes(), path.stat().st_mtime_ns) if path.is_file() else (None, None)
    backup = output / 'before-install.zip'
    with zipfile.ZipFile(backup, 'w', zipfile.ZIP_DEFLATED) as archive:
        for name, (data, _) in snapshot.items():
            if data is not None:
                archive.writestr(name, data)
    log(backup, 'Respaldar archivos propios antes de actualizar Default')
    closed()
    # Detect any external change before the first mutation, then recheck each file.
    for name, (data, modified) in snapshot.items():
        path = contained(INSTALL, name)
        if data is not None:
            if not path.is_file() or path.read_bytes() != data or path.stat().st_mtime_ns != modified:
                raise RuntimeError('Installed file changed during installation: ' + name)
        elif path.exists():
            raise RuntimeError('Installed path appeared during installation: ' + name)
    for name in sorted(set(planned) | owned):
        path = contained(INSTALL, name)
        data, modified = snapshot[name]
        if data is not None:
            if not path.is_file() or path.read_bytes() != data or path.stat().st_mtime_ns != modified:
                raise RuntimeError('Installed file changed during installation: ' + name)
        elif path.exists():
            raise RuntimeError('Installed path appeared during installation: ' + name)
        if name in planned:
            if data != planned[name]:
                path.parent.mkdir(parents=True, exist_ok=True)
                path.write_bytes(planned[name])
                if path.read_bytes() != planned[name] or path.stat().st_size != len(planned[name]):
                    raise RuntimeError('Copy verification failed: ' + name)
                log(path, 'Instalar archivo propio desde repositorio externo en Default')
        elif data is not None:
            # Only the recorded ownership set may lose files. Unknown files stay.
            path.unlink()
            log(path, 'Retirar archivo propio obsoleto; bytes preservados en backup de instalacion')
    if any(contained(INSTALL, name).read_bytes() != data for name, data in planned.items()):
        raise RuntimeError('Final installation verification failed.')
    if state_before is not None:
        if (state.read_bytes(), state.stat().st_mtime_ns) != state_before:
            raise RuntimeError('Ownership manifest changed during installation.')
    elif state.exists():
        raise RuntimeError('Ownership manifest appeared during installation.')
    state.write_text(json.dumps({'generated_at': result['document_generated_at'], 'files': sorted(planned)}, indent=2) + '\n', encoding='utf-8')
    log(state, 'Actualizar inventario de propiedad para futuras instalaciones')
    result.update(action='install', installed=True, installation=str(INSTALL), backup=str(backup))
    print('Installed and byte-verified in existing Default:', INSTALL, flush=True)

def package(output, result, destination):
    if input_hash() != result['input_sha256']:
        raise RuntimeError('Source changed after validation.')
    stage = output / 'package-stage'
    for name, source in runtime().items():
        target = contained(stage, name)
        target.parent.mkdir(parents=True, exist_ok=True)
        target.write_bytes(source.read_bytes())
    (stage / DLL).write_bytes(Path(result['built_dll']).read_bytes())
    (stage / 'src').mkdir()
    (stage / 'src/Plugin.cs').write_bytes((REPO / 'src/Plugin.cs').read_bytes())
    destination = destination or WORKSPACE / 'salidas/thunderstore' / datetime.now(ZoneInfo('Europe/Madrid')).strftime('%Y-%m-%d') / output.name
    filename = f"{json.loads((stage / 'manifest.json').read_text(encoding='utf-8-sig'))['namespace']}-SuccClan-{result['version']}-beta.zip"
    if (destination / filename).exists():
        raise RuntimeError('Refusing to overwrite an existing release ZIP.')
    spec = importlib.util.spec_from_file_location('succclan_packager', REPO / 'tools/package_thunderstore.py')
    packager = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(packager)
    target = packager.package(stage, destination)
    with zipfile.ZipFile(target) as archive:
        for name in archive.namelist():
            if archive.read(name) != (stage / name.removeprefix('plugins/')).read_bytes():
                raise RuntimeError('Package differs from staged validated content: ' + name)
    result.update(action='package', zip=str(target), zip_sha256=digest(target.read_bytes()), installed=False)
    log(target, 'Preparar ZIP desde build validado y revision identificada; no instalar ni publicar')
    print('Prepared for manual upload:', target, flush=True)

def main():
    global INSTALL
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('action', choices=('validate', 'install', 'package'))
    parser.add_argument('--output', type=Path, help='ZIP directory; package action only')
    args = parser.parse_args()
    if args.output and args.action != 'package':
        parser.error('--output is only valid with package')
    if args.action == 'install':
        INSTALL = select_default_runtime()
        closed()
    output, result = validate()
    if args.action == 'install':
        install(output, result)
    elif args.action == 'package':
        package(output, result, args.output)
    report = output / 'result.json'
    report.write_text(json.dumps(result, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    log(report, 'Registrar accion completada y hashes; published false')
    print(json.dumps(result, ensure_ascii=False, indent=2))

if __name__ == '__main__':
    main()

# 2026-11-07-1206||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py||Separar validar, instalar Default y preparar ZIP; verificar copias y preservar archivos ajenos

# 2026-11-07-1208||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py||Corregir saltos reales en informes y registro; verificar inventario de instalacion y añadir pruebas de aislamiento

# 2026-11-07-1318||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py||Verificar todas las instantaneas antes de la primera escritura de instalacion

# 2026-11-07-1325||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py||Resolver la copia Default ya ejecutable sin activar DLL .old ni alterar configuracion

# 2026-11-07-1431||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py||Usar sufijo beta en nombres de ZIP de la proxima subida
