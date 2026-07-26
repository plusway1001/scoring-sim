// NEW: lets you pick the app's colors from the Inspector instead of editing USS by
// hand. GameScope.uss defines its palette as USS custom properties (--color-accent,
// --color-bg, etc. — see the top of the .page rule there) and every other rule in
// that file references them with var(--color-xxx). Unity has no public API to set a
// USS custom property's value from C# at runtime, so this component instead edits
// the .uss FILE ON DISK (in the Editor only) whenever you change a color here, then
// asks Unity to reimport it — which is what actually makes the whole app re-theme
// itself, including every hover/active/toggled state, since those all reference the
// same variables. See THEMING.md at the project root for setup + how this works.
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
#endif

namespace GameScope
{
    [ExecuteAlways]
    public class ThemeSettings : MonoBehaviour
    {
        [Tooltip("Drag the GameScope.uss asset here (Assets/UI Toolkit/GameScope.uss).")]
        public StyleSheet targetStyleSheet;

        [Header("Backgrounds")]
        public Color background = new Color32(10, 10, 18, 255);
        public Color panelBackground = new Color32(18, 18, 30, 255);

        [Header("Border")]
        public Color border = new Color32(42, 42, 62, 255);

        [Header("Accent")]
        public Color accent = new Color32(233, 69, 96, 255);
        public Color accentDark = new Color32(197, 45, 70, 255);

        [Header("Text")]
        public Color textColor = new Color32(255, 255, 255, 255);
        public Color textMuted = new Color32(136, 136, 136, 255);
        public Color textFaint = new Color32(85, 85, 85, 255);

        [Header("Status colors (score bands, success/warning/error)")]
        public Color good = new Color32(0, 229, 160, 255);
        public Color goodDark = new Color32(0, 199, 140, 255);
        public Color mid = new Color32(245, 197, 24, 255);
        public Color bad = new Color32(224, 92, 92, 255);

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyToStyleSheet();
        }

        [ContextMenu("Apply Colors Now")]
        private void ApplyToStyleSheet()
        {
            if (targetStyleSheet == null) return;

            string path = AssetDatabase.GetAssetPath(targetStyleSheet);
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

            string uss = File.ReadAllText(path);
            string original = uss;

            uss = SetVar(uss, "--color-bg", background);
            uss = SetVar(uss, "--color-panel-bg", panelBackground);
            uss = SetVar(uss, "--color-border", border);
            uss = SetVar(uss, "--color-accent", accent);
            uss = SetVar(uss, "--color-accent-dark", accentDark);
            uss = SetVar(uss, "--color-text", textColor);
            uss = SetVar(uss, "--color-text-muted", textMuted);
            uss = SetVar(uss, "--color-text-faint", textFaint);
            uss = SetVar(uss, "--color-good", good);
            uss = SetVar(uss, "--color-good-dark", goodDark);
            uss = SetVar(uss, "--color-mid", mid);
            uss = SetVar(uss, "--color-bad", bad);

            if (uss == original) return;

            File.WriteAllText(path, uss);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        private static string SetVar(string uss, string varName, Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255f);
            int g = Mathf.RoundToInt(color.g * 255f);
            int b = Mathf.RoundToInt(color.b * 255f);
            string replacement = $"{varName}: rgb({r}, {g}, {b});";
            string pattern = Regex.Escape(varName) + @":\s*rgb\([^)]*\);";
            return Regex.Replace(uss, pattern, replacement);
        }
#endif
    }
}
