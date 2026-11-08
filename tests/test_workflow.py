# Documento generado el 2026-11-07-1208
import importlib.util
from pathlib import Path
import tempfile
import unittest
import json
import zipfile
import sys
sys.dont_write_bytecode = True
spec = importlib.util.spec_from_file_location('develop', Path(__file__).resolve().parents[1] / 'tools/develop.py')
dev = importlib.util.module_from_spec(spec)
spec.loader.exec_module(dev)
class WorkflowTests(unittest.TestCase):
 def setUp(self):
  self.tmp=tempfile.TemporaryDirectory();self.base=Path(self.tmp.name)
  dev.REPO=self.base/'repo';dev.INSTALL=self.base/'Default';dev.REPO.mkdir();dev.INSTALL.mkdir()
  (dev.REPO/'tools').mkdir();(dev.REPO/'.build').mkdir()
  (dev.REPO/'tools/default-runtime-files.json').write_text(json.dumps({'files':['old.json','replaced.json']}))
  (dev.REPO/'new.json').write_bytes(b'new')
  (dev.INSTALL/'old.json').write_bytes(b'old');(dev.INSTALL/'replaced.json').write_bytes(b'before');(dev.INSTALL/'foreign.txt').write_bytes(b'foreign')
  dev.runtime=lambda:{'new.json':dev.REPO/'new.json','replaced.json':dev.REPO/'new.json'}
  dev.input_hash=lambda:'fixed';dev.log=lambda *args:None;dev.closed=lambda:None
  self.output=self.base/'output';self.output.mkdir();self.dll=self.base/'built.dll';self.dll.write_bytes(b'dll')
  self.result={'input_sha256':'fixed','built_dll':str(self.dll),'document_generated_at':'test'}
 def tearDown(self):self.tmp.cleanup()
 def test_install_preserves_foreign_and_removes_only_owned(self):
  dev.install(self.output,self.result)
  self.assertEqual((dev.INSTALL/'foreign.txt').read_bytes(),b'foreign')
  self.assertFalse((dev.INSTALL/'old.json').exists())
  self.assertEqual((dev.INSTALL/'replaced.json').read_bytes(),b'new')
  self.assertEqual((dev.INSTALL/dev.DLL).read_bytes(),b'dll')
  with zipfile.ZipFile(self.output/'before-install.zip') as z:self.assertEqual(z.read('old.json'),b'old');self.assertEqual(z.read('replaced.json'),b'before')
  self.assertTrue(self.result['installed'])
 def test_changed_file_blocks_overwrite(self):
  calls=[]
  def closed():
   calls.append(1)
   if len(calls)==2:(dev.INSTALL/'replaced.json').write_bytes(b'external')
  dev.closed=closed
  with self.assertRaisesRegex(RuntimeError,'changed during installation'):dev.install(self.output,self.result)
  self.assertEqual((dev.INSTALL/'replaced.json').read_bytes(),b'external')
  self.assertTrue((dev.INSTALL/'old.json').exists())
  self.assertFalse((dev.INSTALL/'new.json').exists())
  self.assertEqual((dev.INSTALL/'foreign.txt').read_bytes(),b'foreign')
 def test_path_escape_rejected(self):
  with self.assertRaisesRegex(RuntimeError,'Unsafe path'):dev.contained(dev.INSTALL,'../outside.txt')
 def test_changed_source_blocks_install(self):
  dev.input_hash=lambda:'changed'
  with self.assertRaisesRegex(RuntimeError,'Source changed'):dev.install(self.output,self.result)
  self.assertEqual((dev.INSTALL/'replaced.json').read_bytes(),b'before')
 def test_resolves_existing_runtime_without_enabling_old_copy(self):
  other=self.base/'other-copy';other.mkdir();(other/(dev.DLL+'.old')).write_bytes(b'disabled')
  (dev.INSTALL/dev.DLL).write_bytes(b'active')
  self.assertEqual(dev.select_default_runtime([other,dev.INSTALL]),dev.INSTALL)
  self.assertFalse((other/dev.DLL).exists())
 def test_ambiguous_or_disabled_runtime_does_not_enable_anything(self):
  other=self.base/'other-copy';other.mkdir()
  with self.assertRaisesRegex(RuntimeError,'exactly one'):dev.select_default_runtime([other,dev.INSTALL])
  self.assertFalse((other/dev.DLL).exists())
  (other/dev.DLL).write_bytes(b'one');(dev.INSTALL/dev.DLL).write_bytes(b'two')
  with self.assertRaisesRegex(RuntimeError,'exactly one'):dev.select_default_runtime([other,dev.INSTALL])
if __name__=='__main__':unittest.main()

# 2026-11-07-1208||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\test_workflow.py||Probar instalacion aislada, contenido ajeno, backup, obsoletos y rechazo de cambios externos

# 2026-11-07-1318||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\test_workflow.py||Asegurar rechazo de cambios externos sin escrituras parciales

# 2026-11-07-1325||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\test_workflow.py||Probar destino existente y conservar alternativas deshabilitadas sin cambios de configuracion
