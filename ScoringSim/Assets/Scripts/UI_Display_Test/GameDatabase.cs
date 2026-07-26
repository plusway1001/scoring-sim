// === CODE FROM main2/Assets/Scripts/UI_Display_Test/GameDatabase.cs === LINE 1-7 ===
// Verbatim from the team's main2 branch — no logic changes. Part of main2's
// separate test-harness scene (see ScoreDisplayUI.cs next to this file); not wired
// into the live VideoScope/GameScope UI Toolkit app, kept here as-is since it was
// already present in the project before this merge.
using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : MonoBehaviour
{
    public List<GameData> games = new();
}