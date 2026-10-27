namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Minimal counter for Psionic and Mutated. Native StatusEffectState is
    /// abstract, so Trainworks needs this concrete class for a passive marker.
    /// Frantic uses its own StatusEffectFranticState and native attack routing.
    /// </summary>
    class StatusEffectMarkerState : StatusEffectState
    {
    }
}
// 2026-10-27-1536||codex-succclan-targetless||src/code/StatusEffectMarkerState.cs||Corregir documentación obsoleta: marcador solo para Psionic y Mutated, no Frantic
