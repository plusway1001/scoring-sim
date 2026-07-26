using GameScope.Scoring;

namespace GameScope
{
    /// <summary>
    /// This used to hold its own placeholder scoring formula. It has been rewired to
    /// call the team's real main2 scoring code (Scripts/Scoring/GeneralScoreCalculator.cs
    /// and Scripts/Scoring/UserScoreCalculator.cs, dropped in unmodified) via the
    /// ScoringBridge adapter (Scripts/Scoring/ScoringBridge.cs). Every page controller
    /// that already calls ScoreCalculator.CalcGeneralScore/CalcUserScore keeps working
    /// unchanged — only what happens *inside* these two methods changed.
    /// === CODE FROM main2/Assets/Scripts/Scoring/ScoreManager.cs === LINE 1-18 ===
    /// (GetGeneralScore/GetUserScore below mirror ScoreManager's two methods 1:1.)
    /// </summary>
    public static class ScoreCalculator
    {
        /// <summary>General (non-personalized) score, 0-100. Now backed by main2's
        /// GeneralScoreCalculator.Calculate(GameData).</summary>
        public static int CalcGeneralScore(GameEntry game)
        {
            var bridgedGame = ScoringBridge.ToGameData(game);
            float score = GeneralScoreCalculator.Calculate(bridgedGame);
            return (int)System.Math.Round(score);
        }

        /// <summary>Personalized score for a given user, or the general score if user is
        /// null/guest. Now backed by main2's UserScoreCalculator.Calculate(GameData, UserData).</summary>
        public static int CalcUserScore(GameEntry game, UserProfile user)
        {
            if (user == null) return CalcGeneralScore(game);

            var bridgedGame = ScoringBridge.ToGameData(game);
            var bridgedUser = ScoringBridge.ToUserData(user);
            ScoreBreakdown breakdown = UserScoreCalculator.Calculate(bridgedGame, bridgedUser);
            return (int)System.Math.Round(breakdown.finalScore);
        }

        /// <summary>Full modifier-by-modifier breakdown for the Game Detail page,
        /// straight from main2's UserScoreCalculator so the "Your Score Modifiers"
        /// panel reflects exactly what main2's formula computed (no re-derivation).</summary>
        public static ScoreBreakdown CalcUserScoreBreakdown(GameEntry game, UserProfile user)
        {
            var bridgedGame = ScoringBridge.ToGameData(game);
            var bridgedUser = ScoringBridge.ToUserData(user);
            return UserScoreCalculator.Calculate(bridgedGame, bridgedUser);
        }
    }
}
