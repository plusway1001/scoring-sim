// =====MARCO SECTION=====
using GameScope.Scoring;

namespace GameScope
{
    public static class ScoreCalculator
    {
        public static int CalcGeneralScore(GameEntry game)
        {
            var bridgedGame = ScoringBridge.ToGameData(game);
            float score = GeneralScoreCalculator.Calculate(bridgedGame);
            return (int)System.Math.Round(score);
        }

        public static int CalcUserScore(GameEntry game, UserProfile user)
        {
            if (user == null) return CalcGeneralScore(game);

            var bridgedGame = ScoringBridge.ToGameData(game);
            var bridgedUser = ScoringBridge.ToUserData(user);
            ScoreBreakdown breakdown = UserScoreCalculator.Calculate(bridgedGame, bridgedUser);
            return (int)System.Math.Round(breakdown.finalScore);
        }

        public static ScoreBreakdown CalcUserScoreBreakdown(GameEntry game, UserProfile user)
        {
            var bridgedGame = ScoringBridge.ToGameData(game);
            var bridgedUser = ScoringBridge.ToUserData(user);
            return UserScoreCalculator.Calculate(bridgedGame, bridgedUser);
        }
    }
}

// =====END OF MARCO SECTION=====
