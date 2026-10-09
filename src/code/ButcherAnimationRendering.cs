using HarmonyLib;
using TrainworksReloaded.Base.Prefab;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;
using System;
using Anim = CharacterUI.Anim;
using AnimNote = CharacterUI.AnimNote;

namespace mt2_succclan.Plugin
{
    /// <summary>Match static sprite sizing for Butcher's frame animation.</summary>
    [HarmonyPatch(typeof(GameObjectCharacterArtFinalizer), "FinalizeGameObject")]
    internal static class ButcherAnimationRendering
    {
        [HarmonyPostfix]
        private static void AfterFinalize(IDefinition<GameObject> definition)
        {
            if (definition.Key != MyPluginInfo.PLUGIN_GUID
                || definition.Id != "IncubusButcherCharacterArt") return;
            var ui = definition.Data.transform.Find("CharacterScale/CharacterUI");
            if (ui == null) return;
            var quad = ui.Find("Quad_Default");
            var sprite = ui.GetComponent<SpriteRenderer>()?.sprite;
            if (quad == null || sprite == null
                || quad.GetComponent<CharacterUIMeshAnimatedSprite>() == null) return;

            // Static CharacterUIMesh.Setup multiplies the quad by sprite
            // bounds; CharacterUIMeshAnimatedSprite.Setup omits this step.
            // Trainworks has already applied the configured unit scale here.
            // Apply once during prefab finalization, not on each frame/Setup.
            var dimensions = sprite.bounds.size;
            quad.localScale = Vector3.Scale(quad.localScale,
                new Vector3(dimensions.x, dimensions.y, 1f));
            if (quad.GetComponent<ButcherIdleBreathing>() == null)
                quad.gameObject.AddComponent<ButcherIdleBreathing>().Configure(
                    quad.GetComponent<CharacterUIMeshAnimatedSprite>());
            if (quad.GetComponent<ButcherPoseTiming>() == null)
                quad.gameObject.AddComponent<ButcherPoseTiming>();
            Plugin.Logger.LogInfo("Incubus Butcher animated quad sized to sprite bounds: " + dimensions);
        }
    }

    // Allow a readable visual recovery without delaying combat callbacks.
    public sealed class ButcherPoseTiming : MonoBehaviour
    {
        private float readableUntil;
        private bool idlePending;

        internal void Started(Anim animation)
        {
            idlePending = false;
            readableUntil = Time.time + (animation == Anim.Attack ? 0.48f
                : animation == Anim.HitReact ? 0.50f : 0f);
        }

        internal bool DeferIdle(Anim requested, Anim current)
        {
            if (requested != Anim.Idle || Time.time >= readableUntil
                || (current != Anim.Attack && current != Anim.HitReact)) return false;
            idlePending = true;
            return true;
        }

        private void LateUpdate()
        {
            if (!idlePending || Time.time < readableUntil) return;
            idlePending = false;
            var model = GetComponent<CharacterUIMeshAnimatedSprite>();
            if (model != null && (model.GetCurrentAnim() == Anim.Attack
                || model.GetCurrentAnim() == Anim.HitReact))
                model.PlayAnimLoop(Anim.Idle, 0f, null);
        }

        private void OnDisable() { idlePending = false; readableUntil = 0f; }
    }

    [HarmonyPatch(typeof(CharacterUIMeshAnimatedSprite), "GetAnimSpeedMultiplier")]
    internal static class ButcherReadableSpeed
    {
        [HarmonyPostfix]
        private static void AfterSpeed(CharacterUIMeshAnimatedSprite __instance,
            Anim anim, ref float __result)
        {
            if ((anim == Anim.Attack || anim == Anim.HitReact)
                && __instance.GetComponent<ButcherPoseTiming>() != null) __result = 1f;
        }
    }

    [HarmonyPatch(typeof(CharacterUIMeshAnimatedSprite), nameof(CharacterUIMeshAnimatedSprite.PlayAnim),
        new Type[] { typeof(Anim), typeof(float), typeof(float), typeof(Action<AnimNote>) })]
    internal static class ButcherPoseStarted
    {
        [HarmonyPrefix]
        private static void BeforePlay(CharacterUIMeshAnimatedSprite __instance, Anim animType)
            => __instance.GetComponent<ButcherPoseTiming>()?.Started(animType);
    }

    [HarmonyPatch(typeof(CharacterUIMeshAnimatedSprite), nameof(CharacterUIMeshAnimatedSprite.PlayAnimLoop),
        new Type[] { typeof(Anim), typeof(float), typeof(Action<AnimNote>) })]
    internal static class ButcherPoseRecovery
    {
        [HarmonyPrefix]
        private static bool BeforeLoop(CharacterUIMeshAnimatedSprite __instance,
            Anim animType, Action<AnimNote>? animCallback)
        {
            // Never suppress a loop with a callback, or a death/new attack.
            return animCallback != null || __instance.GetComponent<ButcherPoseTiming>()
                ?.DeferIdle(animType, __instance.GetCurrentAnim()) != true;
        }
    }

    /// <summary>Continuous breathing on one idle pose, anchored at the feet.</summary>
    public sealed class ButcherIdleBreathing : MonoBehaviour
    {
        [SerializeField] private CharacterUIMeshAnimatedSprite? animator;
        [SerializeField] private Vector3 restingScale;
        [SerializeField] private Vector3 restingPosition;
        [SerializeField] private float footY;
        private float idleStarted;
        private bool wasIdle;

        internal void Configure(CharacterUIMeshAnimatedSprite model)
        {
            animator = model;
            restingScale = transform.localScale;
            restingPosition = transform.localPosition;
            var mesh = GetComponent<MeshFilter>()?.sharedMesh;
            if (mesh != null)
            {
                // Every frame uses the same 450px canvas, with feet at y=434.
                var bounds = mesh.bounds;
                footY = bounds.min.y + bounds.size.y * (16f / 450f);
            }
        }

        private void LateUpdate()
        {
            // Idle is enum value 1 in Trainworks; all other clips keep their
            // native timing, dimensions and clip offsets.
            bool idle = animator != null && (int)animator.GetCurrentAnim() == 1;
            if (!idle)
            {
                if (wasIdle) transform.localScale = restingScale;
                wasIdle = false;
                return;
            }
            if (!wasIdle) idleStarted = Time.time;
            wasIdle = true;
            float phase = (Time.time - idleStarted) * (2f * Mathf.PI / 3.8f);
            float breath = Mathf.Sin(phase) * 0.004f;
            transform.localScale = Vector3.Scale(restingScale,
                new Vector3(1f + breath * 0.35f, 1f + breath, 1f));
            transform.localPosition = restingPosition + new Vector3(0f,
                -footY * restingScale.y * breath, 0f);
        }

        private void OnDisable()
        {
            if (animator == null) return;
            transform.localScale = restingScale;
            if (wasIdle) transform.localPosition = restingPosition;
            wasIdle = false;
        }
    }
}
