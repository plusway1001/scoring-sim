// === CODE FROM main2/Assets/Scripts/Data/GameData.cs === (partial, as found already dropped into this project) ===
// This file (and Data/UserData.cs next to it) was already sitting in the GameScope
// project before this merge, unwired — same situation as the Scoring/ folder was.
// It couldn't compile as-is for two reasons, both fixed here with no change to the
// actual data fields or any scoring/gameplay logic:
//   1. Its class name `GameData` collided with the plain GameData/UserData bridge
//      types added in Scripts/Scoring/ScoringBridge.cs (those are what
//      GeneralScoreCalculator.cs/UserScoreCalculator.cs actually run against — see
//      MERGE_NOTES.md). Wrapped in the GameScope.Legacy namespace below so both can
//      coexist; nothing in the active app currently references this namespaced copy.
//   2. Its OnValidate() referenced `ScoreDisplayUI`, a class from main2's separate
//      UI_Display_Test test harness that isn't part of this project — commented out
//      below rather than left as a compile error, since it's an editor-only debug
//      hook, not scoring/gameplay logic.
using System.Collections.Generic;
using UnityEngine;

namespace GameScope.Legacy
{
    [System.Serializable]
    public class GameData : MonoBehaviour
    {
        public string gameID;
        public string title;
        public Sprite Icon;
        public Sprite Logo;
        public string websiteURL;

        public string primaryGenre;
        public List<string> tags = new();

        public string ageRating;

        public float price;

        public int releaseYear;

        // General Score Inputs

        public float criticScore;          //0-100

        public float communityAverage;     //0-10

        public int reviewCount;

        public string developer;

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
