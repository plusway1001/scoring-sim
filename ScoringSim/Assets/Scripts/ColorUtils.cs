using UnityEngine;

namespace VideoScope
{
    public static class ColorUtils
    {
        /// <summary>Wraps ColorUtility.TryParseHtmlString for hex strings like "#1a1a3e".</summary>
        public static bool TryParse(string hex, out Color color)
        {
            return ColorUtility.TryParseHtmlString(hex, out color);
        }
    }
}
