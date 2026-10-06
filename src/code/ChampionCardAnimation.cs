using System;
using HarmonyLib;
using TrainworksReloaded.Base.Prefab;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace mt2_succclan.Plugin
{
    /// <summary>Attach to champion and rare card-art prefabs, never character art.</summary>
    [HarmonyPatch(typeof(GameObjectCardArtDecorator), nameof(GameObjectCardArtDecorator.Setup))]
    internal static class SuccChampionCardAnimationPatch
    {
        [HarmonyPostfix]
        private static void AfterSetup(IDefinition<GameObject> definition)
        {
            if (!string.Equals(definition.Key, MyPluginInfo.PLUGIN_GUID, StringComparison.Ordinal)
                || definition.Configuration.GetSection("type").Value != "card_art") return;
            if (!TryProfile(definition.Id, out var hue, out var focus, out var secondFocus,
                out float period, out float pulseSpeed)) return;
            var root = definition.Data;
            var image = root.transform.Find("CardSprite")?.GetComponent<Image>();
            if (image == null || root.GetComponent<SuccChampionCardAnimation>() != null) return;
            // Clip only the art rectangle; the frame and card text are siblings
            // owned by CardUI. Tiny portrait movement must not cross their edge.
            if (root.GetComponent<RectMask2D>() == null) root.AddComponent<RectMask2D>();
            var overlay = new GameObject("SuccChampionCardAtmosphere", typeof(RectTransform), typeof(CanvasRenderer));
            overlay.layer = root.layer;
            overlay.transform.SetParent(root.transform, false);
            var rectangle = overlay.GetComponent<RectTransform>();
            rectangle.anchorMin = Vector2.zero;
            rectangle.anchorMax = Vector2.one;
            rectangle.offsetMin = Vector2.zero;
            rectangle.offsetMax = Vector2.zero;
            var atmosphere = overlay.AddComponent<SuccChampionCardAtmosphere>();
            atmosphere.raycastTarget = false;
            atmosphere.Configure(hue, focus, secondFocus, pulseSpeed);
            root.AddComponent<SuccChampionCardAnimation>().Configure(image, atmosphere, period);
        }

        // Explicit IDs prevent the presentation from touching relics, other
        // clans, character art, or cards which are not rare in this release.
        private static bool TryProfile(string id, out Color hue, out Vector2 focus,
            out Vector2 secondFocus, out float period, out float pulseSpeed)
        {
            hue = new Color(0.65f, 0.20f, 1f);
            focus = new Vector2(0.5f, 0.5f);
            secondFocus = new Vector2(-1f, -1f);
            period = 7f;
            pulseSpeed = 1.4f;
            switch (id)
            {
                case "KnightMareCardArt":
                    hue = new Color(0.12f, 0.72f, 1f); focus = new Vector2(0.29f, 0.77f); period = 6.6f; break;
                case "ShadowLadyCardArt":
                    hue = new Color(1f, 0.22f, 0.78f); focus = new Vector2(0.68f, 0.89f); period = 7.4f; break;
                case "CubusSpikeArt":
                    focus = new Vector2(0.68f, 0.14f); secondFocus = new Vector2(0.26f, 0.85f); pulseSpeed = 2.2f; break;
                case "DepressionWhisperArt":
                    focus = new Vector2(0.40f, 0.73f); hue = new Color(0.52f, 0.48f, 1f); period = 8.8f; pulseSpeed = 0.85f; break;
                case "IllusionTwinsArt":
                    hue = new Color(0.12f, 0.85f, 1f); focus = new Vector2(0.74f, 0.48f); secondFocus = new Vector2(0.72f, 0.22f); period = 7.8f; break;
                case "InsanityReachArt":
                    hue = new Color(1f, 0.12f, 0.20f); focus = new Vector2(0.24f, 0.71f); secondFocus = new Vector2(0.77f, 0.67f); pulseSpeed = 1.8f; break;
                case "ParadoxTomeArt":
                    hue = new Color(0.15f, 0.78f, 1f); focus = new Vector2(0.40f, 0.39f); secondFocus = new Vector2(0.73f, 0.35f); period = 6.8f; break;
                case "PlagueBoostArt":
                    hue = new Color(0.43f, 1f, 0.12f); focus = new Vector2(0.84f, 0.32f); secondFocus = new Vector2(0.55f, 0.73f); period = 8.2f; pulseSpeed = 1.1f; break;
                case "AbyssPrincessCardArt":
                    hue = new Color(1f, 0.14f, 0.70f); focus = new Vector2(0.79f, 0.50f); secondFocus = new Vector2(0.74f, 0.21f); period = 8f; break;
                case "ArroganceGhostCardArt":
                    hue = new Color(1f, 0.26f, 0.89f); focus = new Vector2(0.49f, 0.51f); secondFocus = new Vector2(0.43f, 0.22f); period = 6.4f; break;
                case "EndlessShadowCardArt":
                    focus = new Vector2(0.71f, 0.21f); secondFocus = new Vector2(0.27f, 0.51f); period = 8.6f; pulseSpeed = 1f; break;
                case "ShadowWarriorCardArt":
                    focus = new Vector2(0.50f, 0.42f); secondFocus = new Vector2(0.24f, 0.61f); period = 7.2f; pulseSpeed = 1.7f; break;
                default: return false;
            }
            return true;
        }
    }

    /// <summary>Animate existing art in UI space without new images or shared materials.</summary>
    public sealed class SuccChampionCardAnimation : MonoBehaviour
    {
        [SerializeField] private Image? portrait;
        [SerializeField] private SuccChampionCardAtmosphere? atmosphere;
        [SerializeField] private Vector3 originalScale;
        [SerializeField] private Vector2 originalPosition;
        [SerializeField] private float period;
        private float startTime;

        internal void Configure(Image image, SuccChampionCardAtmosphere overlay, float cycle)
        {
            portrait = image;
            atmosphere = overlay;
            originalScale = image.rectTransform.localScale;
            originalPosition = image.rectTransform.anchoredPosition;
            period = cycle;
            startTime = Time.unscaledTime;
        }

        private void OnEnable() => startTime = Time.unscaledTime;

        private void LateUpdate()
        {
            // Prefab templates have no canvas. Hidden/offscreen card instances
            // need no per-frame graphic rebuild, and no gameplay RNG is used.
            if (portrait == null || portrait.canvas == null || portrait.canvasRenderer.cull) return;
            float elapsed = Time.unscaledTime - startTime;
            float phase = elapsed * (2f * Mathf.PI / Mathf.Max(1f, period));
            float breathing = Mathf.Sin(phase);
            float zoom = 1.028f + 0.008f * breathing;
            var rectangle = portrait.rectTransform;
            rectangle.localScale = Vector3.Scale(originalScale, new Vector3(zoom, zoom, 1f));
            // Minimum overscan is 1% per side; drift stays below 0.4% so the
            // motion never uncovers an empty edge of the square portrait.
            rectangle.anchoredPosition = originalPosition + new Vector2(
                Mathf.Sin(phase * 0.5f) * rectangle.rect.width * 0.003f,
                Mathf.Cos(phase) * rectangle.rect.height * 0.004f);
            // Respect CardUI's own tint/alpha (disabled card, fade, hover).
            // The light pulse lives in the overlay, not in the card's color.
            if (atmosphere != null) atmosphere.color = portrait.color;
            atmosphere?.Advance(elapsed);
        }

        private void OnDisable()
        {
            if (portrait == null) return;
            portrait.rectTransform.localScale = originalScale;
            portrait.rectTransform.anchoredPosition = originalPosition;
        }
    }

    /// <summary>Small UI glows and embers, drawn as a mesh, confined to card art.</summary>
    public sealed class SuccChampionCardAtmosphere : MaskableGraphic
    {
        [SerializeField] private Color glowColor;
        [SerializeField] private Vector2 focus;
        [SerializeField] private Vector2 secondFocus;
        [SerializeField] private float pulseSpeed;
        private float elapsed;
        internal void Configure(Color hue, Vector2 primary, Vector2 secondary, float speed)
        {
            glowColor = hue; focus = primary; secondFocus = secondary;
            pulseSpeed = speed; SetVerticesDirty();
        }
        internal void Advance(float time) { elapsed = time; SetVerticesDirty(); }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            var area = rectTransform.rect;
            float unit = Mathf.Min(area.width, area.height);
            if (unit <= 0f) return;
            var hue = glowColor;
            hue.r *= color.r;
            hue.g *= color.g;
            hue.b *= color.b;
            float pulse = 0.5f + 0.5f * Mathf.Sin(elapsed * pulseSpeed);
            // Match the existing magic held in each portrait, rather than
            // placing a glow over the champion's face.
            AddGlow(mesh, At(area, focus.x, focus.y),
                unit * (0.052f + pulse * 0.017f), hue, (0.12f + pulse * 0.10f) * color.a);
            if (secondFocus.x >= 0f)
                AddGlow(mesh, At(area, secondFocus.x, secondFocus.y),
                    unit * (0.038f + (1f - pulse) * 0.014f), hue,
                    (0.10f + (1f - pulse) * 0.08f) * color.a);
            for (int i = 0; i < 12; i++)
            {
                float progress = Mathf.Repeat(elapsed * (0.10f + (i % 3) * 0.015f) + i * 0.137f, 1f);
                float side = (i % 2 == 0) ? 0.13f : 0.87f;
                float x = side + 0.055f * Mathf.Sin(progress * 2f * Mathf.PI + i * 2.1f);
                float y = 0.07f + progress * 0.86f;
                float fade = Mathf.Sin(progress * Mathf.PI);
                AddGlow(mesh, At(area, x, y), unit * (0.008f + (i % 3) * 0.002f),
                    hue, fade * (0.22f + (i % 2) * 0.12f) * color.a);
            }
        }

        private static Vector2 At(Rect area, float x, float y)
            => new(area.xMin + x * area.width, area.yMin + y * area.height);

        private static void AddGlow(VertexHelper mesh, Vector2 center, float radius, Color hue, float alpha)
        {
            const int segments = 16;
            int first = mesh.currentVertCount;
            hue.a = alpha;
            mesh.AddVert(new Vector3(center.x, center.y, 0f), hue, Vector2.zero);
            hue.a = 0f;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * 2f * Mathf.PI / segments;
                mesh.AddVert(new Vector3(center.x + Mathf.Cos(angle) * radius,
                    center.y + Mathf.Sin(angle) * radius, 0f), hue, Vector2.zero);
                if (i > 0) mesh.AddTriangle(first, first + i, first + i + 1);
            }
        }
    }
}
