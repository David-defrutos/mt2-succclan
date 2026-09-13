namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Estado marcador: no hace nada por si mismo, solo acumula cargas.
    ///
    /// Trainworks Reloaded EXIGE un class_name en cada status_effect propio, asi que un
    /// estado "de solo tooltip" no se puede declarar unicamente en JSON. Esta clase vacia
    /// es ese minimo: hereda de StatusEffectState y no sobreescribe nada.
    ///
    /// La lleva puesta psionic (contador de KnightMare) y frantic (marcador del debuff,
    /// cuyo efecto real lo aplican dazed + witherbloom desde las cartas).
    /// </summary>
    class StatusEffectMarkerState : StatusEffectState
    {
    }
}
