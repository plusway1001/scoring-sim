using System;
using System.Linq;

namespace VideoScope
{
    public static class ScoreCalculator
    {
        /// <summary>General (non-personalized) score, 0-100ish.</summary>
        public static int CalcGeneralScore(GameEntry game)
        {
            double vol = Math.Min(Math.Log(game.TotalRatings) / Math.Log(70000) * 15, 15);
            double nostalgia = Math.Max(0, (2024 - game.Year) / 30.0) * 10;
            double score = game.CriticScore * 0.4 + game.CommunityAvg * 10 * 0.35 + vol + nostalgia;
            return (int)Math.Round(score);
        }

        /// <summary>Personalized score for a given user, or the general score if user is null/guest.</summary>
        public static int CalcUserScore(GameEntry game, UserProfile user)
        {
            if (user == null) return CalcGeneralScore(game);

            if (user.Ratings.TryGetValue(game.Id, out int explicitRating))
                return explicitRating * 10;

            double score = CalcGeneralScore(game);

            string[] ratingOrder = { "G", "PG13", "R16", "R18" };
            int gameRatingIdx = Array.IndexOf(ratingOrder, game.Rating);
            int userMaxIdx = Array.IndexOf(ratingOrder, user.MaxRating);
            if (gameRatingIdx > userMaxIdx) return 0;

            if (user.LikedGenres.Contains(game.Genre)) score += 20;
            if (user.DislikedGenres.Contains(game.Genre)) score -= 20;

            int tagMatches = game.Tags.Count(t => user.LikedTags.Contains(t));
            score += Math.Min(tagMatches * 2, 10);

            if (game.Price >= user.PriceMin && game.Price <= user.PriceMax) score += 5;

            if (user.FavCreators.Contains(game.Dev)) score += 10;

            return (int)Math.Max(0, Math.Min(100, Math.Round(score)));
        }
    }
}
