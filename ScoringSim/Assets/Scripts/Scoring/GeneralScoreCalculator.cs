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

        Debug.Log($"Critic: {game.criticScore}");
        Debug.Log($"Community: {game.communityAverage}");
        Debug.Log($"Reviews: {game.reviewCount}");
        Debug.Log($"Year: {game.releaseYear}");
        Debug.Log($"General Score = {score}");

        return Mathf.Clamp(score, 0, 100);
    }
}