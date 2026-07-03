using System;
using UnityEngine.UIElements;

namespace VideoScope
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
    }
}
