public class ScoreManager
{
    public static float GetGeneralScore(GameData game)
    {
        return GeneralScoreCalculator.Calculate(game);
    }

    public static float GetUserScore(GameData game, UserData user)
    {
        return UserScoreCalculator
            .Calculate(game, user)
            .finalScore;
    }

    public static ScoreBreakdown GetScoreBreakdown(GameData game, UserData user)
    {
        return UserScoreCalculator.Calculate(game, user);
    }
}