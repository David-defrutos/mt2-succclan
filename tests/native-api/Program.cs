// Documento generado el 2026-11-07-1205
// Documento generado el 2026-10-27-1501

using Mono.Cecil;
var plugin=ModuleDefinition.ReadModule(args[0]);var game=ModuleDefinition.ReadModule(args[1]);int checks=0;
void Check(bool ok,string m){if(!ok)throw new Exception(m);checks++;}
IEnumerable<TypeDefinition> Types(ModuleDefinition m)=>m.Types.SelectMany(Descend);
IEnumerable<TypeDefinition> Descend(TypeDefinition t)=>new[]{t}.Concat(t.NestedTypes.SelectMany(Descend));
foreach(var type in Types(plugin))foreach(var annotation in type.CustomAttributes.Where(a=>a.AttributeType.FullName=="HarmonyLib.HarmonyPatch")){
 var targetRef=annotation.ConstructorArguments.FirstOrDefault(a=>a.Type.FullName=="System.Type").Value as TypeReference;
 var name=annotation.ConstructorArguments.FirstOrDefault(a=>a.Type.FullName=="System.String").Value as string;
 if(targetRef==null||name==null)continue;
 var target=Types(game).FirstOrDefault(t=>t.FullName==targetRef.FullName);
 if(target==null)continue; // Unity/library patches are outside Assembly-CSharp.
 var methods=target.Methods.Where(m=>m.Name==name).ToList();
 var signature=annotation.ConstructorArguments.FirstOrDefault(a=>a.Type.FullName=="System.Type[]").Value as CustomAttributeArgument[];
 if(signature!=null)methods=methods.Where(m=>m.Parameters.Count==signature.Length&&m.Parameters.Select(p=>p.ParameterType.FullName).SequenceEqual(signature.Select(p=>((TypeReference)p.Value).FullName))).ToList();
 Check(methods.Count==1,$"{type.Name}: native target {targetRef.FullName}.{name} resolves to {methods.Count} methods");
 var native=methods[0];
 foreach(var patch in type.Methods.Where(m=>m.Name is "Before" or "After" or "Postfix" or "Prefix" or "BeforeApply" or "AfterCollect" or "AfterSetup" or "AfterGetSimultaneousCards"))foreach(var parameter in patch.Parameters){
  string n=parameter.Name;
  if(n=="__instance"){Check(parameter.ParameterType.FullName==target.FullName,$"{type.Name}: instance type");continue;}
  if(n=="__result"){Check(parameter.ParameterType.FullName.TrimEnd('&')==native.ReturnType.FullName,$"{type.Name}: return type {parameter.ParameterType} vs {native.ReturnType}");continue;}
  if(n.StartsWith("___")){Check(target.Fields.Any(f=>f.Name==n[3..]&&f.FieldType.FullName==parameter.ParameterType.FullName),$"{type.Name}: field injection {n}");continue;}
  if(n.StartsWith("__"))continue;
  var original=native.Parameters.FirstOrDefault(p=>p.Name==n);
  Check(original!=null&&original.ParameterType.FullName.TrimEnd('&')==parameter.ParameterType.FullName.TrimEnd('&'),$"{type.Name}: native parameter {n} ({parameter.ParameterType})");
 }
}

// UI tooltips must come from existing effects, not broad UI interception or
// reflection delegates initialized when a UI-facing type is first accessed.
var forbiddenUiTargets=new[]{"CardTooltipContainer","CharacterTooltipHelper"};
Check(!Types(plugin).SelectMany(t=>t.CustomAttributes).Where(a=>a.AttributeType.FullName=="HarmonyLib.HarmonyPatch")
 .SelectMany(a=>a.ConstructorArguments).Any(a=>a.Value is TypeReference type&&forbiddenUiTargets.Contains(type.FullName)),
 "no broad CardTooltipContainer/CharacterTooltipHelper patches");
Check(!Types(plugin).SelectMany(t=>t.Methods).Where(m=>m.Name==".cctor"&&m.HasBody)
 .SelectMany(m=>m.Body.Instructions).Any(i=>i.Operand is MethodReference method&&method.Name is "CreateDelegate" or "MethodDelegate" or "HarmonyDelegate"),
 "no private-method reflection delegates bound by static type initialization");
IEnumerable<MethodDefinition> IteratorBody(string typeName,string methodName){
 var type=Types(game).Single(t=>t.FullName==typeName);
 foreach(var factory in type.Methods.Where(m=>m.Name==methodName)){
  var attr=factory.CustomAttributes.FirstOrDefault(a=>a.AttributeType.Name=="IteratorStateMachineAttribute");
  if(attr?.ConstructorArguments.FirstOrDefault().Value is TypeReference state)
   foreach(var method in Types(game).Single(t=>t.FullName==state.FullName).Methods.Where(m=>m.Name=="MoveNext"))yield return method;
 }
}
bool HasCall(IEnumerable<MethodDefinition> methods,string type,string name)=>methods.Any(m=>m.HasBody&&m.Body.Instructions.Any(i=>i.Operand is MethodReference r&&r.DeclaringType.FullName==type&&r.Name==name));
Check(HasCall(IteratorBody("CharacterTooltipHelper","GetAbilityTooltipsImpl"),"CardTooltipHelper","GetCardEffectTooltipContents"),"unit ability effects use the native effect tooltip route");
Check(HasCall(IteratorBody("CharacterTooltipHelper","GetTooltipContentsImpl"),"CharacterTriggerState","ShouldDisplayOnCharacterTooltips"),"native character visibility gate retained before trigger tooltips");
Check(HasCall(IteratorBody("CharacterTooltipHelper","GetTooltipContentsImpl"),"CardTooltipHelper","GetCardEffectTooltipContents"),"unit trigger effects use native effect tooltip route");

if(args.Length>2){
 using var conductor=ModuleDefinition.ReadModule(args[2]);
 var discard=Types(conductor).Single(t=>t.FullName=="Conductor.CardEffects.CardEffectRandomDiscardFromCardPile");
 bool Calls(MethodDefinition m,string type,string name)=>m.HasBody&&m.Body.Instructions.Any(i=>i.Operand is MethodReference r&&r.DeclaringType.FullName==type&&r.Name==name);
 Check(Calls(discard.Methods.Single(m=>m.Name=="FilterTargetCards"),"CardPool","Contains"),"installed Conductor supports pool restriction");
 Check(Calls(discard.Methods.Single(m=>m.Name=="FilterTargetCards"),"CardState","GetCardDataID"),"pool matches card IDs");
 Check(Calls(discard.Methods.Single(m=>m.Name=="TestEffect"),discard.FullName,"FilterTargetCards"),"count test uses filtered candidates");
 var apply=discard.NestedTypes.Single(t=>t.Name.StartsWith("<ApplyEffect>"));var move=apply.Methods.Single(m=>m.Name=="MoveNext");
 Check(Calls(move,discard.FullName,"FilterTargetCards"),"execution filters same card pool");
 Check(Calls(move,"CardManager","DiscardCard"),"Conductor delegates discard to native CardManager");
 Check(Calls(move,"CardEffectState","GetIntInRange"),"execution respects configured discard count");
 var handPatch=Types(conductor).Single(t=>t.FullName=="Conductor.Patches.CustomTargetModes_CardTargetsPatch");
 var handCollect=handPatch.Methods.Single(m=>m.Name=="Postfix");
 Check(Calls(handCollect,"CardManager","GetHand"),"installed Conductor collects Hand cards");
 Check(handPatch.CustomAttributes.Any(a=>a.AttributeType.FullName=="HarmonyLib.HarmonyPatch"&&a.ConstructorArguments.Any(arg=>arg.Value is string name&&name=="CollectCardTargets")),"Conductor Hand collection patches native card collector");
 Check(handCollect.Body.Instructions.Any(i=>i.Operand is FieldReference f&&f.Name=="targetMode")&&handCollect.Body.Instructions.Any(i=>i.Operand is MethodReference r&&r.Name=="get_Count"),"Conductor guards Hand mode and existing targets");
 Check(handCollect.Body.Instructions.Any(i=>i.Operand is MethodReference r&&r.Name=="AddRange"),"Conductor passes hand cards to existing discard filters");
 Check(!Types(plugin).Any(t=>t.Name=="SuccClanHandCardTargetsPatch"),"redundant SuccClan Hand bridge removed");
 var manager=Types(game).Single(t=>t.FullName=="CombatManager");
 Check(manager.Methods.Where(m=>m.Name=="TestEffects").Any(m=>Calls(m,"CardEffectData","GetShouldFailToCastIfTestFails")),"native whole-card rejection flag exists");
 var status=Types(game).Single(t=>t.FullName=="CardEffectAddStatusEffect");
 Check(status.NestedTypes.SelectMany(t=>t.Methods).Where(m=>m.HasBody).Any(m=>m.Body.Instructions.Any(i=>i.Operand is FieldReference f&&f.Name=="overrideImmunity")),"native Pyre effect sets immunity override");
 Check(status.Methods.Single(m=>m.Name=="GetStatusEffectStack").Body.Instructions.Any(i=>i.Operand is MethodReference r&&r.Name=="RandomElement"),"multiple status entries choose randomly: Mutant must use separate effects");
 foreach(string n in new[]{"ButcherAnimationRendering","ButcherReadableSpeed","ButcherPoseStarted","ButcherPoseRecovery"})
  Check(!Types(plugin).Any(t=>t.Name==n),"archived Butcher hooks absent from compiled DLL");
 Check(!Types(plugin).Any(t=>t.Name=="ObsessingSparkNativeIntegration"),"redundant Spark getter patch removed");
 Check(!Types(plugin).Any(t=>t.Name=="SuccChampionCardAnimationPatch"),"Trainworks animation patch replaced by Roundhouse");

 var context=Types(plugin).Single(t=>t.Name=="SuccClanEffectContext");var cardScope=context.Methods.Single(m=>m.Name=="IsCard");var unitScope=context.Methods.Single(m=>m.Name=="IsCharacter");
 Check(Calls(cardScope,"CardState","GetLinkedClassID")&&Calls(cardScope,"CardState","GetAssetName"),"clan scoped by linked class and asset name");
 Check(!Calls(cardScope,"CardState","GetCardDataID"),"no clan prefix assumptions on opaque GUID");
 Check(Calls(unitScope,"GameData","GetAssetKey")||Calls(unitScope,"CharacterData","GetAssetKey"),"character uses source asset key");
 Check(!Types(plugin).Any(t=>t.Name=="CardEffectDiscardSpecificCard"),"duplicated discard implementation removed");
}
if (args.Length > 3)
{
 using var rebuilt = ModuleDefinition.ReadModule(args[3]);
 var candidateMethods = Types(plugin).SelectMany(t => t.Methods).ToDictionary(m => m.FullName);
 var rebuiltMethods = Types(rebuilt).SelectMany(t => t.Methods).ToDictionary(m => m.FullName);
 Check(candidateMethods.Keys.OrderBy(x => x).SequenceEqual(rebuiltMethods.Keys.OrderBy(x => x)), "rebuilt method signatures match candidate");
 foreach (var pair in candidateMethods)
 {
  var left = pair.Value;
  var right = rebuiltMethods[pair.Key];
  Check(left.HasBody == right.HasBody, "method body presence " + pair.Key);
  if (left.HasBody)
   Check(left.Body.Instructions.Select(i => i.ToString()).SequenceEqual(right.Body.Instructions.Select(i => i.ToString())), "rebuilt executable IL " + pair.Key);
 }
 Console.WriteLine($"Rebuilt DLL executable IL matches candidate across {candidateMethods.Count} methods; PDB/source paths and MVID may differ.");
}
Console.WriteLine($"PASS {checks} native Harmony target, result, field and parameter contracts checked directly from installed game metadata.");

// 2026-10-27-1501||codex-succclan-targetless||salidas\build\succclan-api-check-2026-10-27\Program.cs||Crear verificación de firmas Harmony frente al ensamblado real sin ejecutar Unity

// 2026-10-27-1543||codex-succclan-targetless||salidas\build\succclan-api-check-2026-10-27\Program.cs||Verificar contratos de descarte Conductor, mano, fallo de cast, inmunidad Pyre y animaciones desactivadas
// 2026-10-27-1545||codex-succclan-targetless||salidas\build\succclan-api-check-2026-10-27\Program.cs||Contrastar selección aleatoria de estados mediante RandomElement nativo
// 2026-11-05-1730||codex-succclan-targetless||salidas\build\succclan-api-check-2026-10-27\Program.cs||Actualizar contratos a PR #4: ausencia de parches archivados, getter Spark y hook interno de animacion
// 2026-11-05-1738||codex-succclan-targetless||salidas\build\succclan-api-check-2026-10-27\Program.cs||Verificar en IL compilado identidad por clase y asset key sin prefijos en GUID

// 2026-11-07-1205||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\native-api\Program.cs||Consolidar suite local enlazando fuente real del repositorio externo

// 2026-11-07-1318||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\native-api\Program.cs||Permitir comparar firmas e IL ejecutable de DLL candidata con rebuild desde ruta externa
// 2026-11-12-0917||codex-succclan-targetless||repos/MT2/variaciones-mias/mt2-succclan/tests/native-api/Program.cs||Cubrir ausencia de parches UI amplios/delegados privados estaticos y rutas nativas de tooltip habilidad/unidad frente a DLL actual
// 2026-11-17-1957||codex-succclan-targetless||repos/MT2/variaciones-mias/mt2-succclan/tests/native-api/Program.cs||Verificar postfix real de Conductor para mano y ausencia de parche duplicado
