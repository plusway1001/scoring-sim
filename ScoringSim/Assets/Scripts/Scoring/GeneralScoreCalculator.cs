// === CODE FROM main2/Assets/Scripts/Scoring/GeneralScoreCalculator.cs === LINE 1-30 ===
// Verbatim from the team's main2 branch — no logic changes, EXCEPT the 5
// Debug.Log calls below are commented out (not removed). This method runs on
// every score calculation (every catalogue card, every sort comparison), so
// those logs were flooding the console with 900+ lines on a single page load.
using UnityEngine;

public static class GeneralScoreCalculator
{
    public static float Calculate(GameData game)
    {
        float critic = game.criticScore;

        float userAverage = game.communityAverage * 10f;

        float volumeBonus = Mathf.Clamp01(Mathf.Log10(game.reviewCount + 1) / 5f) * 100f;

        int age = System.DateTime.Now.Year - game.releaseYear;

        float nostalgia = Mathf.Clamp(age * 2f, 0, 100);

        float score =
            critic * 0.40f +
            userAverage * 0.35f +
            volumeBonus * 0.15f +
            nostalgia * 0.10f;

        // Debug.Log($"Critic: {game.criticScore}");
        // Debug.Log($"Community: {game.communityAverage}");
        // Debug.Log($"Reviews: {game.reviewCount}");
        // Debug.Log($"Year: {game.releaseYear}");
        // Debug.Log($"General Score = {score}");

        return Mathf.Clamp(score, 0, 100);
    }
}