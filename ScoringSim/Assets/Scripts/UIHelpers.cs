// =====JANE SECTION=====
using System;
using UnityEngine.UIElements;

namespace GameScope
{
    public static class UIHelpers
    {
        public static Button CreateTag(string text, bool active, Action onClick)
        {
            var btn = new Button(onClick) { text = text };
            btn.AddToClassList("tag");
            if (active) btn.AddToClassList("tag--active");
            return btn;
        }

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

        public static string ScoreColorClass(int score)
        {
            if (score <= 0) return "score-badge--na";
            if (score >= 85) return "score-badge--good";
            if (score >= 70) return "score-badge--mid";
            return "score-badge--bad";
        }

        public static void StyleFilledSlider(Slider slider)
        {
            var tracker = slider.Q(className: "unity-base-slider__tracker");
            if (tracker == null) return;

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

        public static void ForceWhiteText(VisualElement field)
        {
            foreach (var te in field.Query<TextElement>().Build())
                te.style.color = UnityEngine.Color.white;
        }

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

// =====END OF JANE SECTION=====
