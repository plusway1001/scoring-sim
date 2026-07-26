// === CODE FROM main2/Assets/Scripts/Data/UserData.cs === (partial, as found already dropped into this project) ===
// Same situation and same two fixes as Data/GameData.cs right next to this file —
// see the header comment there for the full explanation. No data-field or logic
// changes, only: wrapped in GameScope.Legacy to resolve the GameData/UserData name
// collision with Scripts/Scoring/ScoringBridge.cs, and the OnValidate() call into
// main2's separate (not-included-here) ScoreDisplayUI is commented out.
using System.Collections.Generic;
using UnityEngine;

namespace GameScope.Legacy
{
    [System.Serializable]
    public class UserData : MonoBehaviour
    {
        public string username;

        public List<string> likedGenres = new();

        //public List<string> dislikedGenres = new();

        public List<string> likedTags = new();

        public string maxAgeRating;

        //public float preferredMinPrice;

        public float preferredMaxPrice;

        public List<string> favouriteDevelopers = new();

        // Developers the user has rated 8+ before
        public List<GameRating> playHistory = new(); // A Dynamic Design System

        // GameID -> Rating (1-10)

        public Dictionary<string, int> ratings = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            // ScoreDisplayUI ui = FindFirstObjectByType<ScoreDisplayUI>();
            //
            // if (ui != null)
            // {
            //     ui.UpdateUI();
            // }
        }
#endif
    }
}
