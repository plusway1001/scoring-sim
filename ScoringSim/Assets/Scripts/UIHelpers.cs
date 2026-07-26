using System;
using UnityEngine.UIElements;

namespace GameScope
{
    public static class UIHelpers
    {
        /// <summary>Creates a pill-style toggle button, e.g. genre/tag chips.</summary>
        public static Button CreateTag(string text, bool active, Action onClick)
        {
            var btn = new Button(onClick) { text = text };
            btn.AddToClassList("tag");
            if (active) btn.AddToClassList("tag--active");
            return btn;
        }

        /// <summary>Fills a container (a `.score-badge-col` VisualElement from UXML) with a
        /// circular score badge + optional caption label, matching the React ScoreBadge component.</summary>
        public static void BuildScoreBadge(VisualElement container, int score, string sizeClass, string label = null)
        {
            container.Clear();

            if (!string.IsNullOrEmpty(label))
            {
                var lbl = new Label(label);
                lbl.AddToClassList("score-badge-col__label");
                container.Add(lbl);
            }

            var badge = new Label(score == 0 ? "N/A" : score.ToString());
            badge.AddToClassList("score-badge");
            badge.AddToClassList(sizeClass);
            badge.AddToClassList(ScoreColorClass(score));
            container.Add(badge);
        }

        // Matches the React version's color thresholds (0 = blocked/no score, else red/yellow/green bands).
        public static string ScoreColorClass(int score)
        {
            if (score <= 0) return "score-badge--na";
            if (score >= 85) return "score-badge--good";
            if (score >= 70) return "score-badge--mid";
            return "score-badge--bad";
        }

        /// <summary>Gives a plain UI Toolkit Slider a coloured "fill" from the low end up
        /// to the current value (Slider has no built-in progress-fill element, just a
        /// flat tracker line + a dragger thumb), matching the reference design where the
        /// filled portion of the price-range slider is shown in the accent colour instead
        /// of the whole track being one flat grey line. Safe to call once per slider,
        /// right after it's added to the tree.</summary>
        public static void StyleFilledSlider(Slider slider)
        {
            var tracker = slider.Q(className: "unity-base-slider__tracker");
            if (tracker == null) return;

            // ProfilePageController's price slider is a single persistent UXML element
            // re-touched every time "Edit Profile" is toggled (unlike onboarding's,
            // which builds a fresh Slider each time) — reuse the fill element and
            // listener across calls instead of stacking up duplicates, but still
            // refresh the fill's width every call, since callers may have just set
            // the slider's value via SetValueWithoutNotify (which doesn't fire the
            // change event our listener below relies on).
            var fill = tracker.Q(className: "themed-slider__fill");
            if (fill == null)
            {
                slider.AddToClassList("themed-slider");
                fill = new VisualElement();
                fill.AddToClassList("themed-slider__fill");
                tracker.Insert(0, fill);
                slider.RegisterValueChangedCallback(evt => UpdateFillWidth(slider, fill, evt.newValue));
            }

            UpdateFillWidth(slider, fill, slider.value);
        }

        private static void UpdateFillWidth(Slider slider, VisualElement fill, float value)
        {
            float range = slider.highValue - slider.lowValue;
            float pct = range > 0 ? (value - slider.lowValue) / range * 100f : 0f;
            pct = UnityEngine.Mathf.Clamp(pct, 0f, 100f);
            fill.style.width = new StyleLength(new Length(pct, LengthUnit.Percent));
        }

        /// <summary>Forces every actual text-rendering sub-element inside a text field to
        /// white, by C# type instead of guessing USS class names (which have moved
        /// around across Unity versions and weren't reliably reaching the real internal
        /// element via CSS alone). The blinking caret uses the same colour as the text
        /// it sits in, so this fixes the "invisible black cursor on a dark field" issue
        /// too, not just the typed characters.</summary>
        public static void ForceWhiteText(VisualElement field)
        {
            foreach (var te in field.Query<TextElement>().Build())
                te.style.color = UnityEngine.Color.white;
        }

        /// <summary>Adds real placeholder text inside a TextField (shown only while the
        /// field is empty, hidden as soon as the user types anything) — UI Toolkit's
        /// TextField has no reliable built-in placeholder across the Unity versions this
        /// project might be opened in, so this overlays a plain Label instead.</summary>
        public static Label AddPlaceholder(TextField field, string placeholder)
        {
            var input = field.Q(className: "unity-base-field__input") ?? (VisualElement)field;

            var label = new Label(placeholder);
            label.AddToClassList("field-placeholder");
            label.pickingMode = PickingMode.Ignore; // clicks pass through to the real field underneath
            input.Add(label);

            void Refresh() => label.style.display = string.IsNullOrEmpty(field.value) ? DisplayStyle.Flex : DisplayStyle.None;
            Refresh();
            field.RegisterValueChangedCallback(_ => Refresh());
            return label;
        }
    }
}
